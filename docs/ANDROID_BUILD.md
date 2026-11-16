# Building the Android APK

The Android app is in the repository root as the Gradle project.

## Automatic build

Every push to `main` runs GitHub Actions. To start a build manually:
1. Open the repository on GitHub.
2. Select **Actions**.
3. Choose **Build URBEX PC APK**.
4. Select **Run workflow**.
5. Open the completed run and download the `URBEX-PC-debug` artifact.

The artifact contains `app-debug.apk`.

## Current app capabilities

- Opens as a native Android app.
- Lets the user select a Windows ISO using Android's document picker.
- Remembers permission to read the selected ISO.
- Shows the current VM development status and hardware caveats.

## Not implemented yet

This APK is only the Android front end. It does not contain QEMU and cannot boot Windows yet. The next engineering milestone is building/integrating QEMU for Android ARM64, then adding persistent QCOW2 disk creation, VM controls, display/input, and clean shutdown.

Do not install Windows images or keys from untrusted sources.
