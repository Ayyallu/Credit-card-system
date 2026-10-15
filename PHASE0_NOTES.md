# Phase 0 — Groundwork Notes

Tracks findings and work done for [MIGRATION_PLAN.md](MIGRATION_PLAN.md) Phase 0.

## Dead/duplicate code — removed

Confirmed none of the following were referenced by any `.csproj` (i.e., they were not even compiled into the current build), so they were removed outright:

- `CC_Ingenio.Manager/MasterManager - Copy.cs`
- `CC_Ingenio.Web/Controllers/BillingController1.cs`
- `CC_Ingenio.Web/Controllers/BillingController2.cs`
- `CC_Ingenio.Web/Controllers/BillingControllerold.cs`
- `CC_Ingenio.Web/Controllers/HomeController1.cs`
- `CC_Ingenio.Web/Controllers/UserController1.cs`
- `CC_Ingenio.Web/Views/Master/ProductAdd - Copy.cshtml`
- `Web - Copy.config` (stray duplicate at repo root)

## GemCryptor.dll / KeyGeneration.dll — investigation

Searched git history and extracted printable strings from both binaries (no decompilation performed). Findings:

- **No source code for either DLL exists in this repository** — both were added as pre-built binaries in the initial "Add project files" commit.
- **GemCryptor.dll**: internal namespace is actually `GeminiCrypto` (not `GemCryptor` — the filename and namespace differ). Used in `CC_Ingenio.Common/Utils/Utils.cs` and `CC_Ingenio.Data/UnitOfWork.cs` for encryption-related helpers. No embedded company/product name found in assembly attributes (they appear to be left at template defaults).
- **KeyGeneration.dll**: embedded PDB path is `d:\KeyGeneration\KeyGeneration\obj\Release\KeyGeneration.pdb` — this strongly indicates it is a **small, custom, in-house utility** (built from a standalone Visual Studio project named "KeyGeneration" on a developer's machine), not a licensed third-party library. Used in `CC_Ingenio.Web/Controllers/HomeController.cs` and `UserController.cs` for login password encryption/decryption (`KeyGeneration.KeyGeneration.DecryptPassword()`, `KeyGeneration.KeyGeneration1.Encryt()`).
- No company/vendor name could be recovered from either binary's metadata.

**Conclusion:** these are very likely custom code written by a past developer, not third-party vendor libraries — which is actually good news for migration risk (no licensing blocker), but the **source is still missing** from this repo. Before Phase 3 (runtime migration), someone with institutional knowledge should be asked whether the original `KeyGeneration` Visual Studio project or GemCryptor/GeminiCrypto source exists anywhere else (another repo, a shared drive, a former developer's machine). If it cannot be found, both are small enough (20KB and 4.5KB) that reimplementing their password-encryption functionality with a modern library (e.g., `Microsoft.AspNetCore.Identity.PasswordHasher` or `System.Security.Cryptography`) during the auth replacement in Phase 3 is a low-effort fallback — but this should be a deliberate decision, not a surprise discovered mid-migration.

## CI — basic build pipeline

Added `.github/workflows/build.yml`: runs on `windows-latest`, restores NuGet packages, and builds `CC_Ingenio.sln` with MSBuild on every push/PR to `master`/`main`/`phase-*` branches. This is build-verification only — no tests exist yet to run (see Phase 1 in the migration plan).

## Masked/synthetic dataset — not started

This item needs direction before any work can start safely:
- Which modules/flows need sample data for migration testing (e.g., just Authorization + Billing, or the full domain)?
- Where should masked data live (a seed script, a `.bak` restore, fixture files)?
- Who defines the masking rules for RUT/card/account fields — compliance or engineering?

No synthetic data has been generated. Flagging this as open until scoped.
