# Connection String Inventory — Template

> This file is a checklist, not a config file. It does not contain any real connection strings, hostnames, or credentials — those must be supplied separately (a secrets manager, a secure internal wiki, or environment-specific config transforms) and never committed to this repo in plaintext.

## Why this is needed

[CURRENT_VERSION.md](CURRENT_VERSION.md) found that `CC_Ingenio.Repository/App.Config` only contains a leftover local-development placeholder connection string (pointing at `(LocalDb)\v11.0` with a scaffold-default catalog name). The real environments' connection details are not in source control, which blocks validating parity during a future EF5 → EF Core migration (Phase 3 of [MIGRATION_PLAN.md](MIGRATION_PLAN.md)).

`CC_Ingenio.Web/Web.config` separately stores DB settings as individual `appSettings` keys (server, database, user, an **encrypted** password value, provider), rather than a standard `<connectionStrings>` block — worth normalizing during the migration.

## What's needed, per environment (Dev / QA / Staging / Production)

- [ ] SQL Server hostname/instance and port
- [ ] Database name
- [ ] Authentication mode (SQL auth vs. Windows/Integrated Security)
- [ ] Service account name (password should live in a secrets manager, not this repo)
- [ ] SQL Server version/compatibility level actually running today (the EDMX targets SQL Server 2008 compatibility — confirm whether the real DB has since been upgraded)
- [ ] Network access requirements (VPN, firewall rules, private endpoint) needed to reach it from a build/migration environment
- [ ] Owner/contact for each environment's database

## How to supply this safely

- Do **not** paste real hostnames, credentials, or connection strings into chat, commits, or this file.
- Preferred: store per-environment secrets in your existing secrets manager (Azure Key Vault, AWS Secrets Manager, etc.) or CI/CD pipeline secrets, and reference them by name here instead of value.
- If a secrets manager isn't yet in place, that decision itself is a Phase 0/1 dependency worth flagging to the team before Phase 3 begins.

## Status

Not yet filled in — waiting on environment details from whoever owns Dev/QA/Staging/Production infrastructure.
