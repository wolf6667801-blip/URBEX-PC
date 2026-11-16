# QEMU engine integration decision

## Selected baseline

Use the Android ARM64 + x86-64 system-emulation path documented by the Limbo Emulator project as the initial engineering reference.

Upstream developer documentation specifies:
- Android host ABI: `arm64-v8a`
- Guest target: `x86_64-softmmu`
- Android NDK: r23 with Clang
- QEMU versions listed as supported by that build: 5.1.0 and 2.9.1

Upstream:
- https://github.com/limboemu/limbo
- https://github.com/limboemu/limbo/blob/master/README.developers

## Integration sequence

1. Pin and audit the exact upstream Limbo source revision and its GPL-2.0 obligations.
2. Reproduce the upstream native build in CI before changing the engine.
3. Keep the x86-64 QEMU native library and its required shared libraries in the Android app's ARM64 ABI directory.
4. Implement a narrow JNI/controller layer for start, stop, pause, and guest console/display integration.
5. Add qemu-img (or a compatible disk-creation implementation) and create one persistent QCOW2 image in app-private storage.
6. Test a small DOS guest before attempting Windows 10 x64.
7. Only after the smoke test succeeds, add Windows installer workflow and XP-look customization guidance.

## Licensing

Limbo is GPL-2.0. If its code or binaries are incorporated or modified for distribution, comply with the applicable GPL source, notice, and redistribution requirements. Keep upstream notices and license files intact. Do not copy assets or libraries without checking their individual licenses.

## Performance reality

The target Android device is ARM64 while the intended guest is x86-64. The initial path therefore relies on software CPU translation (TCG), not direct x86 hardware virtualization. Expect much lower performance than a native PC. A small DOS guest and classic Doom are the first test; Windows 10 x64 is a later, much heavier test.

## Status

This is an engineering decision and build plan. The QEMU engine has not yet been compiled or integrated into URBEX PC.
