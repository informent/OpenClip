# OpenClip

OpenClip is a local-first Windows clipboard manager. It keeps searchable clipboard history on the device, with no cloud account and no telemetry.

Clipboard history is bounded to prevent silent database growth, while pinned entries are preserved. Private-key blocks, common secret assignments, and checksum-valid payment-card numbers are excluded before persistence. History writes are atomic and retain a recovery copy so a damaged primary database does not erase the last known-good history.

MIT licensed.
