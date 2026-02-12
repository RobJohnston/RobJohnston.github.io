# SA&A Compliance Documentation Agent

## Purpose
Generate Security Assessment and Authorization (SA&A) documentation by scanning the codebase for security control implementation references. Supports the Authority to Operate (ATO) process required for Government of Canada IT systems.

**IMPORTANT DISCLAIMER**: This agent's documentation format is inspired by US federal security assessment practices (NIST SP 800-53). While ITSG-33 security controls are based on NIST SP 800-53, the specific documentation format and deliverables required for Canadian SA&A may differ by department. **Consult your department's SA&A team** to determine the actual documentation format, templates, and submission requirements. This agent provides a starting point that should be adapted to your department's specific SA&A process.

## Supported Frameworks
- **ITSG-33**: IT Security Risk Management framework controls (primary framework for SA&A)
- **TBS Security Policy**: Treasury Board of Canada Secretariat security requirements
- **CCCS Cloud Security Profile**: Canadian Centre for Cyber Security cloud-specific controls

## How It Works

### 1. Annotate Code with Control References

Add security control references in XML documentation comments:

```csharp
/// <summary>
/// Authenticate user against GC Identity Management service.
///
/// ITSG-33 Security Controls:
///     - AC-2: Account Management
///         Implementation: Users managed via central IAM service
///     - AC-3: Access Enforcement
///         Implementation: Role-based access control (RBAC) via session roles
///     - IA-2: Identification and Authentication
///         Implementation: Username/password with MFA for privileged accounts
///     - IA-5: Authenticator Management
///         Implementation: BCrypt.NET password hashing (cost factor 12)
///     - AU-2: Audit Events
///         Implementation: All authentication attempts logged
/// </summary>
public async Task<User> AuthenticateUser(string username, string password)
{
    _logger.LogInformation("authentication_attempt", username);

    // Hash password and compare
    var user = await _userRepository.GetByUsername(username);
    if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
    {
        _logger.LogInformation("authentication_success", user.Id);
        return user;
    }
    else
    {
        _logger.LogWarning("authentication_failure", username);
        return null;
    }
}
```

### 2. Run the Compliance Documentation Agent

```bash
# Scan codebase for control implementations
@saa-compliance scan --classification=ProtectedB --output=docs/saa/
```

### 3. Generated Documentation

The agent produces the following documentation formats (adapt to your department's SA&A requirements):

#### Control Implementation Matrix

```markdown
| Control ID | Control Name | Implementation Status | Evidence | Notes |
|------------|--------------|----------------------|----------|-------|
| AC-2 | Account Management | Implemented | `src/Auth/UserService.cs:45` | Central IAM integration |
| AC-3 | Access Enforcement | Implemented | `src/Auth/AuthorizationAttributes.cs:12` | RBAC via session roles |
| IA-2 | Identification and Authentication | Implemented | `src/Auth/UserService.cs:45` | Username/password + MFA |
| IA-5 | Authenticator Management | Implemented | `src/Auth/UserService.cs:58` | bcrypt hashing (factor 12) |
| AU-2 | Audit Events | Implemented | `src/Logging/AuditLogger.cs:23` | Structured JSON logs |
| AU-3 | Content of Audit Records | Implemented | `src/Logging/AuditLogger.cs:23` | Timestamp, user ID, event type |
| SC-28 | Protection of Information at Rest | Implemented | `src/Services/EncryptionService.cs:34` | AES-256 encryption for PII |
| SC-8 | Transmission Confidentiality | Implemented | `src/Startup.cs:89` | HTTPS enforced, HSTS header |
```

#### Compliance Gap Analysis

```markdown
## Controls Requiring Implementation

### High Priority
- **AU-6**: Audit Review, Analysis, and Reporting
  - Status: Not implemented
  - Required for: Protected B systems
  - Recommendation: Implement automated log analysis with alerting

- **CM-2**: Baseline Configuration
  - Status: Not implemented
  - Required for: SA&A Authority to Operate (ATO)
  - Recommendation: Document infrastructure-as-code baseline in `docs/baseline-config.md`

### Medium Priority
- **IR-4**: Incident Handling
  - Status: Partially implemented
  - Evidence: Incident response plan exists (`docs/incident-response-plan.md`) but no automated detection
  - Recommendation: Implement anomaly detection for suspicious patterns

- **SC-7**: Boundary Protection
  - Status: Partially implemented
  - Evidence: AWS Security Groups / Azure Network Security Groups configured, but not documented
  - Recommendation: Document network boundaries in security architecture diagram
```

#### Security Control Implementation Sections

The agent generates pre-filled sections for SA&A documentation:

```markdown
## 3.4 Security Controls Implementation

### 3.4.1 Access Control (AC)

**AC-2: Account Management**
The system implements centralized account management through integration with Government of Canada Identity Management services. User accounts are provisioned and de-provisioned through the central IAM portal. All account creation, modification, and deletion events are logged to the audit system.

*Evidence*: `src/Auth/UserService.cs:45-67`, `src/Auth/IamIntegration.cs:12-89`

**AC-3: Access Enforcement**
Role-based access control (RBAC) is enforced throughout the application. Users are assigned roles (e.g., 'citizen', 'case_worker', 'admin') and access to resources is controlled via decorators that verify role membership. Unauthorized access attempts are logged and result in HTTP 403 Forbidden responses.

*Evidence*: `src/Auth/AuthorizationAttributes.cs:12-34`, `src/Auth/RbacService.cs:45-123`

### 3.4.2 Identification and Authentication (IA)

**IA-2: Identification and Authentication**
Users authenticate via username and password. Privileged accounts (admin, case_worker) require multi-factor authentication (MFA) via SMS or authenticator app. Session tokens are generated upon successful authentication and stored in Redis with 15-minute expiration.

*Evidence*: `src/Auth/UserService.cs:45-67`, `src/Auth/MfaService.cs:23-89`

**IA-5: Authenticator Management**
Passwords are hashed using bcrypt with cost factor 12. Minimum password requirements: 12 characters, mixed case, numbers, and special characters. Passwords are validated against a list of common passwords (NIST bad password list). Users are required to change passwords every 90 days.

*Evidence*: `src/Auth/PasswordService.cs:34-78`, `config/password-policy.yml`

[... additional controls ...]
```

### 4. Usage Patterns

**Generate initial documentation**:
```bash
@saa-compliance scan --output=docs/saa/controls.md
```

**Update documentation after code changes**:
```bash
@saa-compliance update --compare-with=docs/saa/controls.md
```

**Generate gap analysis for SA&A review**:
```bash
@saa-compliance gaps --required-controls=docs/saa/required-controls.yml
```

**Export for SA&A submission**:
```bash
@saa-compliance export --format=docx --output=docs/saa/security-controls.docx
```

## Control Reference Format

Use this format in XML documentation comments for the agent to parse:

```csharp
/// <summary>
/// [Method description]
///
/// ITSG-33 Security Controls:
///     - {CONTROL_ID}: {Control Name}
///         Implementation: {Brief description of how this code implements the control}
///     - {CONTROL_ID}: {Control Name}
///         Implementation: {Brief description}
/// </summary>
```

Example:
```csharp
/// <summary>
/// Log authentication event with audit trail.
///
/// ITSG-33 Security Controls:
///     - AU-2: Audit Events
///         Implementation: Logs all authentication attempts with timestamp, username, and result
///     - AU-3: Content of Audit Records
///         Implementation: Structured JSON logs include timestamp, event_type, user_id, session_id
/// </summary>
```

## Benefits

1. **Living documentation**: Security documentation stays in sync with code
2. **Faster SA&A reviews**: Auditors get clear evidence trails, accelerating Authority to Operate (ATO)
3. **Compliance visibility**: Developers see which ITSG-33 controls are covered
4. **Gap identification**: Automated detection of missing controls before SA&A submission
5. **Reduced documentation burden**: Generate 70% of SA&A docs automatically

## Limitations

- **Documentation format may not match department requirements**: This agent generates documentation based on US federal practices. Your department may require different formats, templates, or deliverables. Always verify with your SA&A team.
- **Agent cannot verify control effectiveness**: Only identifies implementations, does not assess whether controls are operating effectively
- **Human review required**: Generated documentation must be reviewed for accuracy and completeness by qualified security personnel
- **Some controls cannot be detected in code**: Physical security, organizational policies, and administrative controls require manual documentation
- **Starting point, not complete package**: Generated documentation is a foundation that must be supplemented with architecture diagrams, threat models, and other SA&A deliverables

## Next Steps

After generating compliance documentation:
1. **Consult your department's SA&A team** to understand required documentation format and templates
2. **Adapt the generated documentation** to match your department's specific SA&A requirements
3. **Review generated control mappings** for accuracy and completeness
4. **Add manual sections** for controls not detectable in code (physical security, organizational policies, administrative controls)
5. **Include additional deliverables** such as architecture diagrams, threat models, privacy impact assessments, and Statement of Sensitivity
6. **Submit to departmental SA&A team** for formal review and feedback
7. **Update control annotations** when code changes to keep documentation current

## Department-Specific Customization

Different Government of Canada departments may have different SA&A processes and documentation requirements. Before using this agent:

- **Identify your department's SA&A team** (may be called IT Security, Cyber Security, or Information Security)
- **Request SA&A documentation templates** specific to your department
- **Ask about required deliverables** (e.g., Statement of Sensitivity format, security control matrices, assessment reports)
- **Understand the submission process** (format, timing, review cycles)
- **Customize the agent output** to match your department's templates and requirements
