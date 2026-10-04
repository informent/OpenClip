# OpenClip

OpenClip is a local-first Windows clipboard manager. It keeps searchable clipboard history on the device, with no cloud account and no telemetry.

Clipboard history is bounded to prevent silent database growth, while pinned entries are preserved. Private-key blocks, common secret assignments, and checksum-valid payment-card numbers are excluded before persistence. History writes are atomic and retain a recovery copy so a damaged primary database does not erase the last known-good history.

Version 1.1 serializes database mutations across OpenClip processes and reloads the latest valid history inside the lock before every change. Two running instances can no longer silently overwrite each other's clips, abandoned process locks recover safely, and a damaged primary file is never copied over the last valid recovery database.

Version 1.2 binds history rows to stable clip identities, so adding a new clip or refreshing the filtered list cannot redirect selection-based preview, pin, or copy actions to a different item.

MIT licensed.
