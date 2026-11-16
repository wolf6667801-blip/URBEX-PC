# URBEX PC

URBEX PC is an Android-hosted virtual machine project. Its target is to run a genuine **Windows 10 x64 guest** with a classic Windows XP-inspired appearance.

## Product direction

- Host: Android ARM64
- Guest: Windows 10 x64
- Virtualization engine: QEMU, initially using software translation (TCG)
- Virtual disk: persistent app-private RAW image for the first storage milestone; QCOW2 is planned after qemu-img is integrated
- UI: URBEX PC Android app for VM setup, start/stop, storage, and status
- Appearance inside Windows: XP-inspired wallpaper, Start menu, taskbar, icons, and window styling where safely supported

The Windows XP look is a visual customization. The guest operating system remains Windows 10.

## Important hardware limitation

On an ARM Android phone, an x86-64 Windows guest generally needs CPU instruction translation unless the device supports a compatible hardware-virtualization path. On a phone with 4 GB RAM, Windows 10 x64 may be very slow, unstable, or unable to run usefully because Android also needs memory. The first milestone must be a boot/performance proof of concept before promising a usable desktop or Steam gaming.

## Persistence

The Android app now has an initial storage milestone: it can create an 8 GB logical RAW disk image in app-private storage and remember its path. The image persists across app restarts. It is not yet attached to QEMU, and the current app cannot boot it. QCOW2 creation is planned for the native-engine stage. A forced stop, Android process kill, low-storage event, or device failure can still corrupt disk data; backup/export is not implemented yet.

A forced stop, Android process kill, low-storage event, or device failure can still corrupt data. The app should request a clean guest shutdown and later add export/backup and snapshot support. Snapshots are not a substitute for backups.

## Windows image and licensing

URBEX PC will not bundle Windows installation media or a Windows product key. The user must supply installation media and a valid license. The app should let the user select an ISO from device storage.

Windows 10 reached end of standard support on 14 October 2025. A Windows 10 guest may need additional security planning. Steam compatibility is not guaranteed; individual games have their own requirements.

## Current repository status

The existing `src/` C# Windows Forms program is an early desktop mock-up, not the Android VM. The Android front end can select an ISO and create a persistent RAW disk image. The QEMU engine has not been compiled or integrated, so guest boot is not available yet.

See [docs/ANDROID_VM_PLAN.md](docs/ANDROID_VM_PLAN.md) for the implementation plan.

## License

No license has been selected yet. All rights remain with the repository owner unless a license is added.
