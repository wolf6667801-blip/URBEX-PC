package com.urbexpc

import androidx.activity.ComponentActivity
import android.content.Intent
import android.graphics.Color
import android.graphics.Typeface
import android.net.Uri
import android.os.Bundle
import android.view.Gravity
import android.view.ViewGroup
import android.widget.Button
import android.widget.LinearLayout
import android.widget.ScrollView
import android.widget.TextView
import android.widget.Toast

class MainActivity : ComponentActivity() {
    private val prefs by lazy { getSharedPreferences("urbex_pc", MODE_PRIVATE) }
    private lateinit var isoStatus: TextView
    private lateinit var engineStatus: TextView
    private lateinit var diskStatus: TextView

    private val chooseIso = registerForActivityResult(
        androidx.activity.result.contract.ActivityResultContracts.OpenDocument()
    ) { uri: Uri? ->
        if (uri != null) {
            try {
                contentResolver.takePersistableUriPermission(
                    uri, Intent.FLAG_GRANT_READ_URI_PERMISSION
                )
                prefs.edit().putString("windows_iso_uri", uri.toString()).apply()
                updateIsoStatus(uri)
                Toast.makeText(this, "ISO selected. Its access is remembered.", Toast.LENGTH_LONG).show()
            } catch (e: Exception) {
                Toast.makeText(this, "Could not keep access to this file: ${e.message}", Toast.LENGTH_LONG).show()
            }
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        window.statusBarColor = Color.rgb(10, 35, 85)
        window.navigationBarColor = Color.rgb(8, 25, 58)
        buildScreen()
        val saved = prefs.getString("windows_iso_uri", null)?.let(Uri::parse)
        updateIsoStatus(saved)
        updateDiskStatus()
    }

    private fun dp(value: Int): Int = (value * resources.displayMetrics.density).toInt()

    private fun buildScreen() {
        val root = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            setBackgroundColor(Color.rgb(225, 235, 247))
        }

        val header = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(dp(20), dp(24), dp(20), dp(20))
            setBackgroundColor(Color.rgb(15, 55, 125))
        }
        header.addView(label("URBEX PC", 28, Color.WHITE, true))
        header.addView(label("ANDROID VIRTUAL MACHINE", 12, Color.rgb(205, 225, 255), true))
        root.addView(header)

        val scroll = ScrollView(this)
        val body = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(dp(18), dp(16), dp(18), dp(24))
        }

        body.addView(section("YOUR VIRTUAL COMPUTER"))
        body.addView(infoCard(
            "Windows 10 x64  •  XP-inspired look",
            "The guest will be real Windows 10. The classic XP appearance will be an optional visual customization inside Windows."
        ))

        body.addView(section("INSTALLATION MEDIA"))
        isoStatus = label("No boot ISO selected", 14, Color.rgb(35, 45, 60), false)
        body.addView(isoStatus)
        body.addView(actionButton("Choose boot ISO (FreeDOS or Windows)") {
            chooseIso.launch(arrayOf("application/octet-stream", "application/x-iso9660-image", "*/*"))
        })

        body.addView(section("PERSISTENT VIRTUAL DISK"))
        diskStatus = label("No virtual disk created yet", 14, Color.rgb(35, 45, 60), false)
        body.addView(diskStatus)
        body.addView(actionButton("Create 8 GB virtual disk") {
            createVirtualDisk()
        })
        body.addView(label(
            "The first disk is a sparse RAW image stored in the app's private storage. It survives app restarts, but it is not yet connected to a running QEMU machine.",
            12, Color.rgb(75, 85, 100), false
        ))

        body.addView(section("DEVICE PROFILE"))
        body.addView(infoCard(
            "Low-memory test profile",
            "This phone has 4 GB RAM. We will start with a conservative test configuration. Windows 10 x64 may still be very slow or fail to run usefully because the phone uses an ARM processor."
        ))

        body.addView(section("VIRTUAL MACHINE ENGINE"))
        engineStatus = label(
            "QEMU engine is not integrated in this first build. The app can select and remember an ISO, but it cannot boot Windows yet.",
            14, Color.rgb(125, 65, 10), false
        )
        body.addView(engineStatus)
        body.addView(infoCard(
            "Persistent storage planned",
            "The app can now create an 8 GB RAW disk image. The next storage milestone is qemu-img/QCOW2 integration, followed by attaching the disk to QEMU and implementing safe guest shutdown."
        ))

        body.addView(section("IMPORTANT"))
        body.addView(label(
            "URBEX PC does not include Windows installation files or a product key. Select media you are licensed to use. Steam and games are not guaranteed to work in emulation.",
            13, Color.rgb(55, 65, 80), false
        ))

        scroll.addView(body)
        root.addView(scroll, LinearLayout.LayoutParams(
            ViewGroup.LayoutParams.MATCH_PARENT, 0, 1f
        ))
        setContentView(root)
    }

    private fun label(text: String, size: Int, color: Int, bold: Boolean): TextView =
        TextView(this).apply {
            this.text = text
            textSize = size.toFloat()
            setTextColor(color)
            if (bold) setTypeface(typeface, Typeface.BOLD)
            setPadding(0, dp(5), 0, dp(5))
        }

    private fun section(text: String): TextView =
        label(text, 13, Color.rgb(15, 55, 125), true).apply {
            setPadding(0, dp(20), 0, dp(8))
        }

    private fun infoCard(title: String, detail: String): LinearLayout {
        val card = LinearLayout(this).apply {
            orientation = LinearLayout.VERTICAL
            setPadding(dp(14), dp(12), dp(14), dp(12))
            setBackgroundColor(Color.WHITE)
        }
        card.addView(label(title, 16, Color.rgb(20, 45, 85), true))
        card.addView(label(detail, 13, Color.rgb(65, 75, 90), false))
        val params = LinearLayout.LayoutParams(
            ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT
        )
        params.bottomMargin = dp(8)
        card.layoutParams = params
        return card
    }

    private fun actionButton(text: String, action: () -> Unit): Button =
        Button(this).apply {
            this.text = text
            isAllCaps = false
            setOnClickListener { action() }
            val params = LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT
            )
            params.topMargin = dp(8)
            layoutParams = params
        }

    private fun createVirtualDisk() {
        try {
            val vmDir = java.io.File(filesDir, "vm")
            if (!vmDir.exists() && !vmDir.mkdirs()) {
                throw IllegalStateException("Cannot create VM storage folder")
            }
            val disk = java.io.File(vmDir, "urbexpc-disk.raw")
            if (disk.exists() && disk.length() > 0L) {
                Toast.makeText(this, "Virtual disk already exists", Toast.LENGTH_LONG).show()
                updateDiskStatus()
                return
            }
            val available = android.os.StatFs(filesDir.absolutePath).availableBytes
            if (available < 256L * 1024L * 1024L) {
                throw IllegalStateException("Less than 256 MB free in app storage")
            }
            java.io.RandomAccessFile(disk, "rw").use { it.setLength(8L * 1024L * 1024L * 1024L) }
            prefs.edit().putString("virtual_disk_path", disk.absolutePath).apply()
            updateDiskStatus()
            Toast.makeText(this, "8 GB virtual disk created", Toast.LENGTH_LONG).show()
        } catch (e: Exception) {
            Toast.makeText(this, "Disk creation failed: ${e.message}", Toast.LENGTH_LONG).show()
        }
    }

    private fun updateDiskStatus() {
        if (!::diskStatus.isInitialized) return
        val path = prefs.getString("virtual_disk_path", null)
        val disk = path?.let { java.io.File(it) }
        diskStatus.text = if (disk != null && disk.exists()) {
            "Disk ready: ${disk.name} • ${disk.length() / (1024L * 1024L * 1024L)} GB • app storage"
        } else {
            "No virtual disk created yet"
        }
    }

    private fun updateIsoStatus(uri: Uri?) {
        if (!::isoStatus.isInitialized) return
        isoStatus.text = if (uri == null) {
            "No Windows ISO selected"
        } else {
            "ISO selected: ${uri.lastPathSegment ?: uri}"
        }
    }
}
