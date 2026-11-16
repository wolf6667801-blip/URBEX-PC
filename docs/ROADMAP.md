# URBEX PC roadmap

## Direction: Android app + real Windows 10 x64 guest

- [x] Create repository and initial desktop-shell mock-up
- [x] Choose Windows 10 x64 as the intended guest OS
- [x] Define persistent QCOW2 virtual-disk requirement
- [x] Add Android app front end and ISO document picker
- [x] Add GitHub Actions APK build workflow
- [x] Research Limbo's Android QEMU x86_64-softmmu path
- [x] Add classic Doom as an early, lightweight VM test target
- [x] Add initial persistent 8 GB RAW disk creation in app-private storage
- [ ] Build and integrate the Android ARM64 QEMU native engine
- [ ] Integrate qemu-img and add QCOW2 disk creation/conversion
- [ ] Implement VM process lifecycle and clean shutdown
- [ ] Implement guest display, pointer, keyboard, and audio
- [ ] Boot-test a small DOS guest and classic Doom
- [ ] Boot-test a user-provided Windows 10 x64 ISO
- [ ] Add backup/export and snapshot support
- [ ] Add optional XP-inspired desktop customization guidance

## Engineering constraints

- The Android front end can select an ISO and create an 8 GB logical RAW disk file, but it does not yet contain QEMU and cannot boot any guest.
- An ARM64 phone emulating x86-64 requires CPU translation; expect slow performance.
- The 4 GB device may not have enough free memory for Windows 10 x64.
- Doom is a first emulator smoke test, not proof that Windows 10 or Steam will run well.
- Do not bundle Windows installation media, product keys, or copyrighted Doom game data.

See [ANDROID_VM_PLAN.md](ANDROID_VM_PLAN.md) and [DOOM_TEST.md](DOOM_TEST.md).
