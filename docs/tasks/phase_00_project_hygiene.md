# Phase 0 Task — Project Hygiene

## Goal

Make the repository safe for AI implementation.

## Scope

Docs and repo setup only.

## Tasks

1. Check current repo status.

```powershell
git status
```

2. Ensure `.gitignore` exists.

Required ignore entries:

```gitignore
bin/
obj/
.vs/
*.user
*.suo
.DS_Store
Thumbs.db
*.log
TestResults/
coverage/
```

3. Ensure these files exist:

```text
AGENTS.md
plan.md
docs/developer_feature_map.md
docs/architecture.md
docs/iso_coordinate_system.md
```

4. Run:

```powershell
dotnet build
```

## Do Not

- Do not change game code unless build is broken by project setup.
- Do not implement movement.
- Do not add new packages.

## Acceptance Criteria

- `dotnet build` passes.
- Docs exist.
- `bin/` and `obj/` are ignored.
