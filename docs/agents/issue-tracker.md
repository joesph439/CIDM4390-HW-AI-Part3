# Issue tracker: Local Markdown

Issues and specs live as Markdown files under `.scratch/`.

## Conventions

- One feature per directory: `.scratch/<feature-slug>/`.
- Specs: `.scratch/<feature-slug>/spec.md`.
- Tickets: `.scratch/<feature-slug>/issues/<NN>-<slug>.md`,
  numbered from `01`, with one file per ticket.
- Record triage state in a `Status:` line near the top.
  Use the strings defined in `triage-labels.md`.
- Append comments and conversation history under `## Comments`.
- Resolve issue numbers within their feature directory.
  Use the full relative path when the feature is ambiguous.

## Publishing and fetching

When a skill says "publish to the issue tracker", create or update
the appropriate local Markdown file using these conventions.

When a skill says "fetch the relevant ticket", read the referenced file.

## Wayfinding operations

Used by `/wayfinder`.

- Map: `.scratch/<effort>/map.md`, containing Notes,
  Decisions-so-far, and Fog.
- Child ticket: `.scratch/<effort>/issues/<NN>-<slug>.md`.
- Type: record `research`, `prototype`, `grilling`, or `task`
  in a `Type:` line.
- Blocking: record dependencies as `Blocked by: NN, NN`.
  A ticket is unblocked when every listed blocker is resolved.
- Frontier: choose the first numbered unresolved, unblocked,
  unclaimed ticket.
- Claim: set `Status: claimed` and save before starting work.
- Resolve: append the answer under `## Answer`, set
  `Status: resolved`, and append a summary and ticket link
  to Decisions-so-far in the map.

The `claimed` and `resolved` statuses describe wayfinding progress;
they are additional to the five triage statuses.
