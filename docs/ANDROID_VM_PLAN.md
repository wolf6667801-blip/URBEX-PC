# Android virtual machine implementation plan

## Goal

Build URBEX PC as an Android ARM64 app that manages a real QEMU virtual machine with a Windows 10 x64 guest. The guest can be customized to resemble Windows XP, but it remains Windows 10.

## Proposed architecture

1. **Android front end**
   - Kotlin + Android SDK.
   - First-run setup, ISO picker using Android Storage Access Framework, VM configuration, start/stop controls, and status/error reporting.
   - VM disk stored in app-private persistent storage by default; provide export/backup to user-selected storage.

2. **Virtual machine engine**
   - QEMU built for Android ARM64 using the Android NDK and a maintained source revision.
   - Initial CPU backend: TCG software translation for x86-64 guest instructions.
   - Do not assume KVM acceleration is available: an ARM host cannot directly hardware-virtualize an x86-64 guest.
   - Keep the engine isolated behind a small controller layer so it can be updated independently.

3. **Guest storage**
   - QCOW2 virtual disk, created with a user-selected size.
   - Persistent disk is reused on every boot; do not create a fresh disk at each launch.
   - Use QEMU's safe shutdown path before closing the app.
   - Add snapshots only after basic disk persistence is tested. Provide backup/export because snapshots on the same physical storage do not protect against device failure.

4. **Windows installation**
   - User selects a Windows 10 x64 ISO.
   - URBEX PC does not distribute Windows installation media or product keys.
   - Installation and activation require the user's own valid license and media.
   - First proof of concept should validate boot, keyboard, pointer, display, and disk writes before adding convenience features.

5. **XP-inspired appearance**
   - Keep Windows 10 as the actual guest OS.
   - Offer optional safe customization guidance: Bliss wallpaper, classic-style icons, a classic Start menu/taskbar tool where compatible.
   - Avoid patching protected system files or silently installing third-party theme tools.

## Target-device reality check

The target phone has 4 GB RAM. Android itself consumes a substantial portion, leaving limited memory for a Windows 10 x64 guest. A starting test profile may need around 1.5–2 GB guest RAM, but this is not a promise that Windows will run well; Android may kill the process or the guest may fail to boot. CPU translation can also make the system very slow and increase heat and battery use.

The first release must be a measured proof of concept on the actual phone. If a useful Windows desktop is not achievable, the honest fallback is to use URBEX PC as a remote desktop client connected to a separate Windows PC, rather than advertise impossible local performance.

## Steam and support

Windows 10 standard support ended on 14 October 2025. Steam and individual games have changing OS and hardware requirements; do not advertise compatibility without testing. A Windows 10 guest does not guarantee that Steam, modern games, GPU acceleration, sound, or networking will work acceptably under emulation.

## Definition of the first working milestone

- Android app launches.
- User chooses a Windows 10 x64 ISO.
- App creates a persistent QCOW2 disk.
- QEMU boots the installer.
- User can install Windows and shut it down cleanly.
- On next launch, the installed guest boots with its files and installed apps intact.

## Current status

This document is a plan, not a claim that an Android APK or QEMU engine has already been built. The existing C# Windows Forms files in `src/` are only an early desktop mock-up and are not the Android virtual machine.
