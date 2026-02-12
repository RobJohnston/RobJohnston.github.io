---
applyTo: "**/aws/**,**/infrastructure/**,**/terraform/**"
---

# AWS Protected B Deployment Instructions

This application deploys to AWS through SSC's Cloud Brokering Service framework agreement.

## Framework Agreement

Project uses AWS through SSC's Cloud Brokering Service framework agreement.

## Security Compliance

All AWS deployments for Protected B data must follow:

1. **SSC Cloud Brokering Service** requirements and framework agreements
2. **CCCS Cloud Security Profile** - [Canadian Centre for Cyber Security guidance](https://www.cyber.gc.ca/en/guidance/cloud-security-guidance) for assessing and authorizing cloud services for Protected B workloads
3. **GC Cloud Guardrails** - [Mandatory baseline security controls](https://github.com/canada-ca/cloud-guardrails) that must be implemented within 30 days
4. **ITSG-33** - [IT Security Risk Management framework](https://www.cyber.gc.ca/en/guidance/it-security-risk-management-lifecycle-approach-itsg-33) controls for access control, encryption, logging, and monitoring

**Key CCCS requirements for Protected B**:
- Data encryption at rest and in transit (TLS 1.2+)
- CMVP-validated cryptographic modules where required
- Multi-factor authentication for administrative access
- Network segmentation and security groups
- Logging and monitoring with 2-year retention
- Canadian data residency (Canada region only)

## Required AWS Services

- **Compute**: AWS Lambda or ECS Fargate (containerized .NET applications)
- **Database**: RDS SQL Server (managed) with encryption at rest
- **Storage**: S3 with server-side encryption (AES-256)
- **Logging**: CloudWatch Logs with 90-day retention (TBS requirement)
- **Key Management**: AWS KMS for encryption keys

## Terraform Configuration

Protected B configuration following SSC guidelines:

```hcl
# Protected B configuration following SSC guidelines
resource "aws_s3_bucket" "app_data" {
  bucket = "dept-benefits-prod-data"

  # SSC requirement: encryption at rest
  server_side_encryption_configuration {
    rule {
      apply_server_side_encryption_by_default {
        sse_algorithm = "AES256"
      }
    }
  }

  # SSC requirement: versioning enabled
  versioning {
    enabled = true
  }

  # SSC requirement: logging enabled
  logging {
    target_bucket = aws_s3_bucket.logs.id
    target_prefix = "app-data/"
  }
}
```

## Deployment via CI/CD

GitHub Actions example:

```yaml
# GitHub Actions example
name: Deploy to AWS (SSC)
on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: aws-actions/configure-aws-credentials@v1
        with:
          aws-region: ca-central-1  # Canadian region required
```

## GC Cloud Guardrails Compliance

All deployments must comply with [GC Cloud Guardrails](https://canada-ca.github.io/cloud-guardrails/):

- Multi-factor authentication enabled for console access
- CloudTrail logging enabled for all API calls
- VPC security groups restrict inbound/outbound traffic
- Automated backup enabled for RDS databases

## Regional Requirements

- **Region**: Use `ca-central-1` (Montreal) for Canadian data residency
- **Availability Zones**: Deploy across multiple AZs for high availability

## Security Best Practices

- Never hard-code AWS credentials in source code
- Use IAM roles for service-to-service authentication
- Enable encryption in transit (TLS 1.2+)
- Enable encryption at rest for all data stores
- Implement least-privilege IAM policies
- Enable AWS Config for compliance monitoring
- Enable GuardDuty for threat detection

## Cost Optimization

- Use Reserved Instances or Savings Plans for predictable workloads
- Implement auto-scaling for compute resources
- Use S3 lifecycle policies to archive old data
- Monitor with AWS Cost Explorer
- Tag all resources for cost allocation

## Support Contacts

- **SSC Cloud Brokering**: ssc-cloud-broker@servicecanada.gc.ca
- **AWS Technical Support**: Available through SSC framework agreement
- **Department Cloud Lead**: cloud-lead@department.gc.ca

## Further Reading

- [SSC Cloud Brokering Service](https://www.canada.ca/en/government/system/digital-government/digital-government-innovations/cloud-services.html)
- [GC Cloud Guardrails](https://canada-ca.github.io/cloud-guardrails/)
- [AWS Canada](https://aws.amazon.com/compliance/canada/)
