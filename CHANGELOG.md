# Changelog

## Unreleased

### Changed

- unified public branding under the GymForge name,
- replaced broken social links with a verified source-code link,
- removed the unfinished strength-standards screen from public navigation,
- expanded repository documentation with architecture, tests and deployment trade-offs,
- added continuous integration for backend and frontend checks.

### Fixed

- corrected misleading exercise-type naming and file-name typos,
- prevented authenticated users from opening login and registration pages,
- replaced the generated commented-out application test with a working test.

## 1.0.0 - 2026-07-25

### Added

- end-to-end training plan creation and scheduling,
- workout completion with per-set results,
- dashboard for due sessions and saved plans,
- training statistics with an exercise gallery and selectable progress chart,
- editable user profiles and metric/imperial preferences,
- Docker production stack with health checks.

### Fixed

- readable statistics typography and chart scaling on large screens,
- accurate 1RM percentage table calculations,
- Wilks calculations for values entered in pounds,
- authenticated redirects back to the originally requested page,
- duplicate form identifiers in the Wilks calculator.

### Security

- production database and JWT secrets are no longer stored in the base application configuration,
- production containers use health checks and restricted privilege escalation.
