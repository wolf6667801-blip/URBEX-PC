# URBEX PC roadmap

## Direction: Android app + real Windows x64 guest

- [x] Create repository and initial desktop-shell mock-up
- [x] Choose Windows 10 x64 as the initial intended guest OS
- [x] Define persistent virtual-disk requirement
- [x] Add Android app front end and ISO document picker
- [x] Add GitHub Actions APK build workflow
- [x] Research Limbo's Android QEMU x86_64-softmmu path
- [x] Add classic Doom as an early, lightweight VM test target
- [x] Add initial persistent 8 GB RAW disk creation in app-private storage
- [x] Audit Limbo source age, build requirements, and GPL-2.0 license
- [ ] Reproduce a pinned Android ARM64 QEMU native build in CI
- [ ] Integrate the QEMU native engine
- [ ] Implement VM lifecycle and safe shutdown
- [ ] Implement guest display, touch/mouse, keyboard, and audio
- [ ] Attach persistent disk and add QCOW2 support
- [ ] Boot-test a small DOS guest and classic Doom
- [ ] Boot-test a user-provided Windows x64 ISO
- [ ] Test Steam client compatibility and performance (not guaranteed)
- [ ] Add backup/export and snapshot support
- [ ] Add optional XP-inspired desktop customization guidance

## Engineering constraints

- The current Android front end can select an ISO and create an 8 GB logical RAW disk file, but it does not yet contain QEMU and cannot boot a guest.
- An ARM64 phone emulating x86-64 requires CPU translation; expect slow performance.
- The 4 GB device may not have enough free memory for Windows 10 x64 alongside Android.
- Windows 10 reached end of standard support on 14 October 2025. Do not present it as a currently supported secure OS without additional planning.
- Steam and individual games have separate requirements; a working Windows desktop does not guarantee Steam or games will run acceptably.
- Do not bundle Windows installation media, product keys, or copyrighted Doom game data.

See [ANDROID_VM_PLAN.md](ANDROID_VM_PLAN.md), [DOOM_TEST.md](DOOM_TEST.md), and [QEMU_ENGINE.md](QEMU_ENGINE.md).
