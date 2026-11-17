# Triage labels

Local tickets record their triage role in a `Status:` line.

| Canonical role | Local status | Meaning |
| --- | --- | --- |
| needs-triage | needs-triage | Maintainer needs to evaluate the issue |
| needs-info | needs-info | Waiting on the reporter for information |
| ready-for-agent | ready-for-agent | Fully specified and ready for autonomous implementation |
| ready-for-human | ready-for-human | Requires human implementation |
| wontfix | wontfix | Will not be actioned |

When a skill applies or removes a triage label, update the ticket's
`Status:` line to the corresponding status.

Wayfinding progress statuses are defined in `issue-tracker.md`.
