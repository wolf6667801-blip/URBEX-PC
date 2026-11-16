# QEMU engine integration decision

## Target

Run a genuine x86-64 Windows guest on an ARM64 Android host using QEMU TCG software translation. The first milestone is a small bootable guest; Windows and Steam are later compatibility/performance tests, not guaranteed outcomes.

## Upstream audit: Limbo

Limbo is a possible engineering reference, not a drop-in Android library.

- Repository: https://github.com/limboemu/limbo
- License reported by GitHub: GPL-2.0.
- Audited upstream revision: `887c6a68cd6b414377d7f8071bc12bbc16e59809`.
- The upstream developer guide describes an ARM64 Android host with an `x86_64-softmmu` guest.
- Its documented native build uses Android NDK r23b/Clang, QEMU 5.1.0 or 2.9.1, and separately patched glib, libffi, pixman, and SDL2 dependencies.
- The checked-in Android library Gradle file uses compile SDK 29 and old AndroidX dependencies. The guide and build instructions are dated; the latest master commit returned by GitHub is from 2022.

This means we must not copy its old Android modules directly into the current app and assume they will build with the current Android toolchain. First reproduce the native build in isolated CI, record all source revisions and checksums, then decide whether to adapt the native QEMU layer or use a better-maintained compatible source.

## Integration sequence

1. [x] Inspect Limbo's Android ARM64/x86-64 build instructions and identify its GPL-2.0 license.
2. [x] Pin the audited Limbo reference revision and record the age/toolchain risks.
3. [ ] Reproduce the upstream native build in CI using a pinned NDK and pinned dependency sources.
4. [ ] Verify the resulting ARM64 shared libraries and their transitive dependencies.
5. [ ] Integrate the QEMU native library into the Android app without importing Limbo's legacy UI.
6. [ ] Implement a narrow VM controller for start, stop, pause, and error reporting.
7. [ ] Render guest video and implement touch-to-mouse plus keyboard input.
8. [ ] Attach persistent storage; add QCOW2 only after the native engine works.
9. [ ] Boot-test FreeDOS, then test a legally obtained classic Doom data package.
10. [ ] Attempt a user-provided Windows x64 ISO only after the lightweight guest is stable.
11. [ ] Test Steam client compatibility separately; do not promise usable gaming on a 4 GB ARM phone.

## Licensing

Limbo is GPL-2.0. If its code or binaries are incorporated or modified for distribution, comply with applicable GPL source, notice, and redistribution requirements. Keep upstream notices and license files intact. Audit the individual licenses of bundled dependencies and patches. Do not copy assets or libraries without checking their licenses.

## Performance reality

The target Android device is ARM64 while the intended guest is x86-64. The initial path therefore relies on software CPU translation (TCG), not direct x86 hardware virtualization. Expect performance far below a native PC. A phone with 4 GB RAM also has to run Android and the emulator, leaving limited memory for Windows. A successful DOS boot does not prove Windows or Steam will run well.

## Current status

The Android front end can select an ISO and create an 8 GB logical RAW disk file, but it does not yet contain QEMU and cannot boot any guest. The Limbo source audit is complete; a reproducible native build is not yet complete.
