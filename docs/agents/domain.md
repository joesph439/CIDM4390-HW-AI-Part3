# Domain docs

This repository uses a single-context domain layout.

## Before exploring

- Read `GLOSSARY.md` at the repository root, when present.
- Read decisions in `docs/adr/` relevant to the work.

If these documents do not exist, proceed silently.
The `/domain-modeling` skill creates them as terms and decisions
are resolved.

## Layout

- `GLOSSARY.md`: shared domain vocabulary.
- `docs/adr/`: architectural decision records.

## Use the glossary's vocabulary

Use the glossary's terms when naming domain concepts in issue
titles, proposals, hypotheses, tests, and code.

If a needed concept is missing, reconsider whether it belongs
in the domain vocabulary or note the gap for `/domain-modeling`.

## Surface decision conflicts

If a proposal contradicts an existing ADR, identify that ADR
and explain why the decision should be reconsidered.
