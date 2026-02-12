+++
title = "How AI Coding Assistants Can Leverage Canada's Open Government APIs"
date = "2026-02-12"
draft = true
description = "From dataset discovery to standards-compliant integration: teaching AI assistants to work with Canadian open data"
category = "government"
tags = ["government", "ai", "open-data", "apis", "web-services"]
image = "/images/open-government-apis.jpg"
+++

Picture this: You're building a municipal planning tool that needs census demographics by postal code. Or a weather-aware scheduling system for outdoor services. Or a transparency dashboard showing government contracts and spending.

You know the data exists. The Government of Canada has been publishing open data for years. But where do you even start?

You browse open.canada.ca, clicking through hundreds of datasets organized by department. You find something promising—"Labour force characteristics by province"—but the download is a 50 MB CSV file. You need an API, not a file download. You dig through documentation trying to find out if Statistics Canada has a web service. Eventually you discover the Web Data Service (WDS), but the documentation talks about "vector identifiers" and "SDMX format" and you're not sure how to actually query the data.

Three hours later, you've found some datasets that *might* work, bookmarked a dozen documentation pages, and you still haven't written a single line of integration code.

**Here's what most developers don't realize**: Canada's Open Government API ecosystem isn't just for manual integration—it's a machine-readable catalog that AI coding assistants can query, understand, and integrate automatically. When you combine Canada's comprehensive open data infrastructure with AI assistance, dataset discovery happens in seconds instead of hours, and standards-compliant integration code gets generated automatically.

This is the second blog post in a series about AI coding assistants and Canadian government development. [In the first post](/blog/government-platforms/canadian-ai-assistants/), we explored how platforms like WET-BOEW and the GC Design System make AI assistants more effective by providing bounded vocabulary and predictable patterns. **This post takes a different angle**: Instead of AI assistants following guidance to generate compliant code, we'll explore AI assistants as **active consumers of government data and services**—discovering datasets, generating integration code with real Canadian data, and ensuring compliance with the Government of Canada Standards on APIs.

## Canada's Open Government API Ecosystem

Before we dive into how AI assistants can help, let's understand the landscape. Canada's federal open data infrastructure consists of several interconnected platforms:

### 1. Open Government Portal (open.canada.ca)

The central catalog for all federal government datasets. Built on **CKAN** (Comprehensive Knowledge Archive Network), it provides:

- **Thousands of datasets** across all federal departments and agencies
- **Searchable API** for dataset discovery (no authentication required for public data)
- **Metadata in both official languages** (English and French)
- **Multiple data formats**: CSV, JSON, XML, GeoJSON, SDMX, and more

**API Base URL**: `https://open.canada.ca/data/en/api/3/action/`

**Key endpoints**:
- `package_search?q=<term>` - Search datasets by keyword
- `package_show?id=<dataset-id>` - Get metadata for a specific dataset
- `recently_changed_packages_activity_list` - Activity stream of updated datasets

**Example query**:
```
https://open.canada.ca/data/en/api/3/action/package_search?q=employment+province&rows=10
```

Returns JSON with dataset metadata including title, description, organization, resources (downloadable files), update frequency, and more.

### 2. Statistics Canada Web Data Service (WDS)

Statistics Canada's API for accessing economic, demographic, and census data. Provides:

- **Time series data** using vector identifiers
- **15 API methods** for data retrieval and metadata
- **JSON and SDMX format** responses
- **Comprehensive coverage** of Canadian statistics

**Base URL**: `https://www.statcan.gc.ca/en/developers/wds`

**Common methods**:
- `getDataFromVectorsAndLatestNPeriods` - Get recent data for specific vectors
- `getCubeMetadata` - Get metadata about a data table
- `getFullTableDownloadCSV` - Download entire tables

**Data types**: Labour force statistics, economic indicators, census data, industry statistics, demographic data

### 3. GeoGratis (Natural Resources Canada)

The federal geospatial data catalog providing:

- **Topographic maps and elevation data**
- **Geographic boundaries** (provinces, municipalities, census subdivisions)
- **Satellite imagery and air photos**
- **Spatial search** by bounding box
- **Multiple output formats**: GeoJSON, KML, CSV, Atom Feed

**Base URL**: `https://geogratis.gc.ca/api/`

**Query parameters**:
- `q=<search term>` - Full-text search
- `bbox=west,south,east,north` - Spatial bounding box filtering
- `alt=<format>` - Response format (json, kml, csv, etc.)

### 4. MSC GeoMet (Environment Canada Weather API)

Environment and Climate Change Canada's weather data service:

- **Real-time weather observations and forecasts**
- **Climate data and water data**
- **OpenAPI/Swagger documentation**
- **No authentication required** for public access (unlike most weather APIs)

**Base URL**: `https://api.weather.gc.ca/`

**Documentation**: OpenAPI spec at `https://api.weather.gc.ca/openapi`

### The Standards on APIs

All Government of Canada APIs are expected to follow the [Standards on APIs](https://www.canada.ca/en/government/system/digital-government/digital-government-innovations/government-canada-standards-apis.html), which mandate:

- **RESTful architecture** with resource-oriented URLs
- **JSON as message format** (UTF-8 encoding)
- **ISO 8601 datetime format** in UTC (yyyy-mm-ddThh:mm:ssZ)
- **TLS 1.2 or higher** for all connections
- **Proper HTTP status codes** and error handling
- **API versioning** in URLs (e.g., `/v1/`)
- **Pagination** for large result sets
- **OpenAPI documentation** where possible

These standards create consistency across government APIs—making it easier for both humans and AI assistants to work with them.

## Discovery: Teaching AI Assistants to Find Relevant Datasets

The first challenge in working with open data is **discovery**: finding the right dataset among thousands of options. This is where AI assistants shine.

### The Traditional Approach

Without AI assistance, discovering datasets looks like this:

1. Browse open.canada.ca manually, clicking through categories
2. Try various search terms, hoping to find relevant datasets
3. Read through dozens of dataset descriptions
4. Check each dataset's format, update frequency, and data quality
5. Download sample files to verify the data actually contains what you need
6. Repeat when your first guesses don't pan out

This can take hours or even days for complex data needs.

### The AI-Assisted Approach

With an AI assistant that understands Canada's Open Government API ecosystem, discovery becomes conversational:

**Example interaction**:

```
User: "I need employment data by province for the last 5 years"

AI Assistant: "I'll search open.canada.ca for relevant datasets..."

[AI queries CKAN API]
GET https://open.canada.ca/data/en/api/3/action/package_search?q=employment+province&rows=20

[AI analyzes results and responds]

AI: "I found 3 relevant datasets:

1. **Labour force characteristics by province** (StatCan Table 14-10-0287-03)
   - Monthly data, seasonally adjusted
   - Available via Statistics Canada WDS API with vector identifiers
   - Last updated: 2026-02-07
   - Coverage: All provinces, 2020-2026
   - Format: JSON via WDS API (no CSV download needed)

2. **Employment by occupation and province** (Open Data Portal)
   - Annual data
   - CSV and JSON formats
   - Coverage: 2015-2025
   - File download only (no API)

3. **Employment by industry and region** (StatCan)
   - Quarterly data
   - SDMX format via WDS
   - Includes territories

The first option (Table 14-10-0287-03) seems best for your 5-year requirement
with monthly granularity. It's available via the Statistics Canada WDS API,
which means you can query it programmatically instead of downloading large files.

Would you like me to generate the C# integration code for this dataset?"
```

### How This Works

The AI assistant can:

1. **Query the CKAN API** programmatically to search datasets
2. **Parse JSON responses** to extract relevant metadata
3. **Compare datasets** by update frequency, coverage, format, and authority
4. **Identify the authoritative source** (e.g., Statistics Canada vs. other departments)
5. **Recommend the best option** based on your requirements
6. **Suggest the appropriate API** (WDS for StatCan data, direct downloads for others)

Here's what the discovery code looks like:

```csharp
using System.Net.Http.Json;

namespace GC.OpenData.Discovery
{
    /// <summary>
    /// Client for searching datasets on open.canada.ca via the CKAN API.
    /// </summary>
    public class OpenDataSearchService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://open.canada.ca/data/en/api/3/action/";
        private readonly ILogger<OpenDataSearchService> _logger;

        public OpenDataSearchService(
            HttpClient httpClient,
            ILogger<OpenDataSearchService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Search for datasets on open.canada.ca.
        /// </summary>
        /// <param name="query">Search term (e.g., "employment province")</param>
        /// <param name="maxResults">Maximum number of results to return</param>
        /// <returns>List of matching datasets</returns>
        public async Task<List<Dataset>> SearchDatasetsAsync(
            string query,
            int maxResults = 10)
        {
            var url = $"{BaseUrl}package_search?q={Uri.EscapeDataString(query)}&rows={maxResults}";

            _logger.LogInformation(
                "Searching open.canada.ca for: {Query}, max results: {MaxResults}",
                query,
                maxResults
            );

            var response = await _httpClient.GetFromJsonAsync<CkanResponse>(url);

            if (response?.Success != true || response.Result == null)
            {
                _logger.LogWarning("CKAN API returned unsuccessful response");
                return new List<Dataset>();
            }

            _logger.LogInformation(
                "Found {Count} datasets matching '{Query}'",
                response.Result.Count,
                query
            );

            return response.Result.Results
                .Select(r => new Dataset
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Notes,
                    Organization = r.Organization.Title,
                    LastModified = r.MetadataModified,
                    Resources = r.Resources.Select(res => new DataResource
                    {
                        Url = res.Url,
                        Format = res.Format,
                        Language = res.Language
                    }).ToList()
                }).ToList();
        }
    }

    // Response models matching CKAN API structure
    public class CkanResponse
    {
        public bool Success { get; set; }
        public CkanResult Result { get; set; }
    }

    public class CkanResult
    {
        public int Count { get; set; }
        public List<CkanPackage> Results { get; set; }
    }

    public class CkanPackage
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }  // Description
        public CkanOrganization Organization { get; set; }
        public DateTime MetadataModified { get; set; }
        public List<CkanResource> Resources { get; set; }
    }

    public class CkanOrganization
    {
        public string Title { get; set; }
    }

    public class CkanResource
    {
        public string Url { get; set; }
        public string Format { get; set; }
        public List<string> Language { get; set; }
    }

    // Simplified models for application use
    public class Dataset
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Organization { get; set; }
        public DateTime LastModified { get; set; }
        public List<DataResource> Resources { get; set; }
    }

    public class DataResource
    {
        public string Url { get; set; }
        public string Format { get; set; }
        public List<string> Language { get; set; }
    }
}
```

### Benefits of AI-Assisted Discovery

**Natural language queries**: Ask "Find datasets about government spending in 2025" instead of learning CKAN query syntax.

**Automatic filtering**: AI can filter by relevance, recency, data quality, and authoritative source without you having to review every result.

**Cross-referencing**: AI can check multiple sources (CKAN catalog, Statistics Canada, GeoGratis) to find the most authoritative dataset.

**Bilingual search**: Finds datasets regardless of whether they're indexed in English or French.

**Time saved**: What used to take hours of manual browsing now takes seconds of conversational query.

## Integration: Generating Standards-Compliant API Clients

Once you've found the right dataset, the next challenge is **integration**: writing code to fetch and process the data. This is where AI assistants excel at generating boilerplate that follows the Standards on APIs.

### The Challenge

Each government API has different patterns:

- **CKAN** uses simple GET requests with query parameters
- **Statistics Canada WDS** uses POST requests with JSON bodies and vector identifiers
- **GeoGratis** supports spatial queries with bounding boxes
- **MSC GeoMet** follows OpenAPI specifications

Manually writing clients for each API means:
- Reading API documentation to understand request/response formats
- Handling authentication (if required)
- Implementing error handling and retry logic
- Parsing government-specific formats (SDMX, GeoJSON, CKAN metadata)
- Ensuring compliance with Standards on APIs
- Adding bilingual metadata handling
- Including proper logging and monitoring

### The AI-Assisted Approach

An AI assistant with knowledge of Canada's Standards on APIs can generate complete, standards-compliant clients automatically.

**Example interaction**:

```
User: "Generate a client for Statistics Canada's Web Data Service to fetch
labour force data by vector ID"

AI Assistant: "I'll create a client that follows the Government of Canada
Standards on APIs. This will include:
- RESTful patterns with proper HTTP methods
- JSON request/response handling
- ISO 8601 datetime format
- Structured logging (no PII)
- Bilingual metadata support
- Proper error handling with abstracted technical details
- XML documentation with compliance references

Here's the implementation..."
```

### Statistics Canada WDS Client Example

```csharp
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace GC.StatisticsCanada
{
    /// <summary>
    /// Client for Statistics Canada Web Data Service API.
    /// Implements GC Standards on APIs (REST, JSON, error handling).
    ///
    /// Documentation: https://www.statcan.gc.ca/en/developers/wds
    /// </summary>
    public class StatCanWdsClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StatCanWdsClient> _logger;
        private const string BaseUrl = "https://www.statcan.gc.ca/api/wds/v1";

        public StatCanWdsClient(
            HttpClient httpClient,
            ILogger<StatCanWdsClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Get data for specific vectors over the latest N periods.
        /// API Method: getDataFromVectorsAndLatestNPeriods
        /// </summary>
        /// <param name="vectorIds">List of vector identifiers (e.g., "v2062815")</param>
        /// <param name="periods">Number of recent periods to retrieve</param>
        /// <param name="language">Language for metadata ("en" or "fr")</param>
        /// <returns>Data points for each vector</returns>
        public async Task<StatCanDataResponse> GetVectorDataAsync(
            List<string> vectorIds,
            int periods,
            string language = "en")
        {
            // Validate inputs per Standards on APIs
            if (vectorIds == null || !vectorIds.Any())
                throw new ArgumentException("At least one vector ID required", nameof(vectorIds));

            if (periods < 1 || periods > 1000)
                throw new ArgumentException("Periods must be between 1 and 1000", nameof(periods));

            if (language != "en" && language != "fr")
                throw new ArgumentException("Language must be 'en' or 'fr'", nameof(language));

            // Build request (StatCan WDS uses POST with JSON body)
            var request = new
            {
                vectorIds = vectorIds.Select(v => v.Replace("v", "")).ToArray(),
                latestN = periods
            };

            try
            {
                // Log request (no PII - public data only)
                _logger.LogInformation(
                    "Requesting StatCan data for {VectorCount} vectors, {Periods} periods, language: {Language}",
                    vectorIds.Count,
                    periods,
                    language
                );

                var url = $"{BaseUrl}/getDataFromVectorsAndLatestNPeriods?lang={language}";
                var response = await _httpClient.PostAsJsonAsync(url, request);

                // Standards on APIs: Proper HTTP status code handling
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadFromJsonAsync<StatCanDataResponse>();

                if (data?.Status != 200)
                {
                    throw new ExternalServiceException(
                        "Statistics Canada API returned error status");
                }

                _logger.LogInformation(
                    "Successfully retrieved {DataPoints} data points",
                    data.Object?.VectorDataPoints?.Sum(v => v.Value?.Count ?? 0) ?? 0
                );

                return data;
            }
            catch (HttpRequestException ex)
            {
                // Standards on APIs: Abstract internal details from errors
                _logger.LogError(ex, "Failed to retrieve StatCan data");
                throw new ExternalServiceException(
                    "Unable to retrieve Statistics Canada data. Please try again later.",
                    ex
                );
            }
        }

        /// <summary>
        /// Get metadata for a specific data cube (table).
        /// API Method: getCubeMetadata
        /// </summary>
        /// <param name="productId">Product ID (e.g., "14100287" for table 14-10-0287)</param>
        /// <param name="language">Language for metadata ("en" or "fr")</param>
        public async Task<CubeMetadata> GetCubeMetadataAsync(
            string productId,
            string language = "en")
        {
            if (string.IsNullOrWhiteSpace(productId))
                throw new ArgumentException("Product ID required", nameof(productId));

            try
            {
                var url = $"{BaseUrl}/getCubeMetadata?productId={productId}&lang={language}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<CubeMetadata>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to retrieve cube metadata for product {ProductId}", productId);
                throw new ExternalServiceException(
                    "Unable to retrieve dataset metadata. Please try again later.",
                    ex
                );
            }
        }
    }

    // Response models matching StatCan WDS JSON structure
    public class StatCanDataResponse
    {
        public int Status { get; set; }
        public DataObject Object { get; set; }
    }

    public class DataObject
    {
        public Dictionary<string, VectorData> VectorDataPoints { get; set; }
    }

    public class VectorData
    {
        public List<DataPoint> Value { get; set; }
    }

    public class DataPoint
    {
        /// <summary>
        /// Reference period in ISO 8601 format (e.g., "2026-01-01")
        /// Per GC Standards on APIs: ISO 8601 datetime format
        /// </summary>
        public string RefPer { get; set; }

        public decimal Value { get; set; }

        /// <summary>
        /// Data quality symbol (e.g., "A" = excellent, "B" = very good)
        /// See StatCan documentation for symbol definitions
        /// </summary>
        public string Symbol { get; set; }
    }

    public class CubeMetadata
    {
        public string ProductId { get; set; }
        public string CubeTitleEn { get; set; }
        public string CubeTitleFr { get; set; }
        public List<Dimension> Dimensions { get; set; }
    }

    public class Dimension
    {
        public int DimensionId { get; set; }
        public string DimensionNameEn { get; set; }
        public string DimensionNameFr { get; set; }
    }

    /// <summary>
    /// Exception for external service failures.
    /// Per Standards on APIs: Abstract internal technical details.
    /// </summary>
    public class ExternalServiceException : Exception
    {
        public ExternalServiceException(string message) : base(message) { }
        public ExternalServiceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
```

### What the AI Assistant Added Automatically

Notice what the generated code includes without you having to ask:

✅ **XML documentation** with references to official APIs and compliance standards

✅ **Input validation** following API constraints (periods between 1-1000, language en/fr)

✅ **Structured logging** with no PII (just counts and metadata)

✅ **Proper error handling** that abstracts technical details (per Standards on APIs)

✅ **Bilingual metadata support** (language parameter, bilingual property names)

✅ **ISO 8601 datetime format** in response models (RefPer field)

✅ **Custom exception types** that follow Standards on APIs error handling guidance

✅ **Clear method signatures** with summary documentation

This is code that would pass a Government of Canada code review on the first try—because it follows the Standards on APIs from the start.

## Compliance: Validating Against Standards on APIs

Generating code is one thing. Ensuring it actually complies with the Government of Canada Standards on APIs is another. This is where AI assistants can act as automated reviewers.

### The Standards on APIs Compliance Checklist

The [Standards on APIs](https://www.canada.ca/en/government/system/digital-government/digital-government-innovations/government-canada-standards-apis.html) mandate several requirements:

**Architecture**:
- ✅ RESTful model with resource-oriented URLs (nouns, not verbs)
- ✅ Appropriate HTTP verbs (GET for retrieval, POST for creation, etc.)
- ✅ JSON as message format (UTF-8 encoding)

**Security**:
- ✅ TLS 1.2 or higher for all connections
- ✅ JWT for API authentication
- ✅ API keys in headers, not URLs
- ✅ Never transmit sensitive data over unencrypted connections

**Data Standards**:
- ✅ ISO 8601 datetime format in UTC (yyyy-mm-ddThh:mm:ssZ)
- ✅ JSON responses must be objects, not arrays
- ✅ Consistent casing (underscore or CamelCase, not mixed)

**Versioning**:
- ✅ Format: `v<Major>.<Minor>.<Patch>`
- ✅ URL reflects major version only (e.g., `/v3/`)
- ✅ Support at least one previous major version

**Error Handling**:
- ✅ HTTP status codes (200 OK, 400 Bad Request, 404 Not Found, 500 Internal Server Error)
- ✅ Abstract internal technical details (no stack traces in responses)
- ✅ Consistent error response format

**Performance**:
- ✅ Pagination required for large result sets
- ✅ Restrict wildcard queries
- ✅ Published performance benchmarks

### AI-Assisted Compliance Review

An AI assistant can review code against this checklist automatically.

**Example interaction**:

```
User: "Review this API controller for Standards on APIs compliance"

[User pastes code]

AI Assistant: "I found 3 violations of the Government of Canada Standards on APIs:

1. **API Key in URL** (Line 24):
   - Current: GET /api/applications/search?apiKey=xxx
   - Standard: 'API keys must pass through headers, not URLs'
   - Fix: Remove apiKey parameter, use [Authorize] attribute with JWT bearer token
   - Security risk: API keys in URLs are logged in server logs and browser history

2. **Non-standard datetime format** (Line 47):
   - Current: "SubmissionDate": "12/02/2026"
   - Standard: 'ISO 8601 datetime format in UTC (yyyy-mm-ddThh:mm:ssZ)'
   - Fix: Use DateTime type, which JSON serializer will format as ISO 8601
   - Example: "SubmissionDate": "2026-02-12T00:00:00Z"

3. **Stack trace in error response** (Line 63):
   - Current: Returning ex.ToString() in error response
   - Standard: 'Abstract internal technical details from error responses'
   - Fix: Return generic error message, log details server-side
   - Security risk: Stack traces expose internal system architecture

Would you like me to generate the corrected code?"
```

### Compliance Review Example

Here's what non-compliant vs. compliant code looks like:

**❌ Non-Compliant Code**:

```csharp
[ApiController]
[Route("api/applications")]
public class ApplicationsController : ControllerBase
{
    // ❌ BAD: API key in URL instead of header
    [HttpGet("search")]
    public async Task<ActionResult> Search(string query, string apiKey)
    {
        if (apiKey != _configuration["ApiKey"])
            return Unauthorized();

        var results = await _service.SearchAsync(query);
        return Ok(results);  // ❌ BAD: Returns array, not object
    }

    // ❌ BAD: Non-standard datetime format
    public class ApplicationDto
    {
        public string SubmissionDate { get; set; } = "12/02/2026";  // Not ISO 8601
    }

    // ❌ BAD: Exposing technical details in errors
    [HttpPost]
    public async Task<ActionResult> Submit([FromBody] ApplicationDto app)
    {
        try
        {
            await _service.SubmitAsync(app);
            return Ok();
        }
        catch (Exception ex)
        {
            // ❌ BAD: Stack trace exposed to client
            return StatusCode(500, ex.ToString());
        }
    }
}
```

**✅ Compliant Code** (generated by AI assistant):

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GC.BenefitsPortal.Controllers
{
    /// <summary>
    /// Applications API Controller.
    ///
    /// GC Compliance:
    ///     - Standards on APIs: RESTful, JSON responses, ISO 8601 dates
    ///     - TLS 1.2+ required (configured in Program.cs)
    ///     - JWT authentication via [Authorize] attribute
    /// </summary>
    [ApiController]
    [Route("api/v1/applications")]  // ✅ GOOD: Version in URL
    [Produces("application/json")]
    [Authorize]  // ✅ GOOD: JWT authentication in headers
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _service;
        private readonly ILogger<ApplicationsController> _logger;

        public ApplicationsController(
            IApplicationService service,
            ILogger<ApplicationsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Search applications by query term.
        /// </summary>
        /// <param name="query">Search term</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Results per page (default: 20, max: 100)</param>
        /// <returns>Paginated search results</returns>
        [HttpGet("search")]  // ✅ GOOD: RESTful URL (no apiKey parameter)
        [ProducesResponseType(typeof(SearchResultsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SearchResultsDto>> Search(
            [FromQuery] string query,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            // ✅ GOOD: Input validation
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new ErrorResponse
                {
                    Error = "Query parameter is required",
                    Code = "INVALID_QUERY"
                });

            if (pageSize > 100)
                return BadRequest(new ErrorResponse
                {
                    Error = "Page size cannot exceed 100",
                    Code = "INVALID_PAGE_SIZE"
                });

            var results = await _service.SearchAsync(query, page, pageSize);

            // ✅ GOOD: Returns object with metadata, not raw array
            return Ok(new SearchResultsDto
            {
                Data = results,
                Meta = new MetadataDto
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = results.TotalCount,
                    Timestamp = DateTime.UtcNow  // ✅ GOOD: Will serialize as ISO 8601
                }
            });
        }

        /// <summary>
        /// Submit a new application.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApplicationResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApplicationResponseDto>> Submit(
            [FromBody] ApplicationDto application)
        {
            try
            {
                // ✅ GOOD: No PII in logs (just application ID)
                _logger.LogInformation(
                    "Application submission started: {ApplicationId}",
                    application.ApplicationId
                );

                var result = await _service.SubmitAsync(application);

                _logger.LogInformation(
                    "Application submitted successfully: {ApplicationId}, Confirmation: {ConfirmationNumber}",
                    application.ApplicationId,
                    result.ConfirmationNumber
                );

                // ✅ GOOD: HTTP 201 Created with location header
                return CreatedAtAction(
                    nameof(GetApplication),
                    new { id = result.ApplicationId },
                    new ApplicationResponseDto
                    {
                        ApplicationId = result.ApplicationId,
                        ConfirmationNumber = result.ConfirmationNumber,
                        SubmittedAt = DateTime.UtcNow,  // ✅ GOOD: ISO 8601
                        Status = "submitted"
                    }
                );
            }
            catch (ValidationException ex)
            {
                // ✅ GOOD: Business validation errors (400 Bad Request)
                _logger.LogWarning(ex, "Application validation failed: {ApplicationId}", application.ApplicationId);
                return BadRequest(new ErrorResponse
                {
                    Error = "Application validation failed",
                    Code = "VALIDATION_ERROR",
                    Details = ex.ValidationErrors
                });
            }
            catch (Exception ex)
            {
                // ✅ GOOD: Abstract technical details, log full exception server-side
                _logger.LogError(ex,
                    "Application processing failed: {ApplicationId}",
                    application.ApplicationId
                );

                // ✅ GOOD: Generic error message, no stack trace
                return StatusCode(500, new ErrorResponse
                {
                    Error = "We could not process your application. Please try again or contact support.",
                    Code = "PROCESSING_ERROR"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationDto>> GetApplication(Guid id)
        {
            // Implementation...
            return Ok();
        }
    }

    // ✅ GOOD: Consistent response format for errors
    public class ErrorResponse
    {
        public string Error { get; set; }
        public string Code { get; set; }
        public object Details { get; set; }
    }

    public class SearchResultsDto
    {
        // ✅ GOOD: Data in "data" property, metadata separate
        public List<ApplicationDto> Data { get; set; }
        public MetadataDto Meta { get; set; }
    }

    public class MetadataDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        // ✅ GOOD: ISO 8601 datetime (automatic via JSON serializer)
        public DateTime Timestamp { get; set; }
    }

    public class ApplicationDto
    {
        public Guid ApplicationId { get; set; }

        // ✅ GOOD: ISO 8601 datetime
        public DateTime SubmissionDate { get; set; }

        public string ServiceType { get; set; }
    }

    public class ApplicationResponseDto
    {
        public Guid ApplicationId { get; set; }
        public string ConfirmationNumber { get; set; }

        // ✅ GOOD: ISO 8601 datetime
        public DateTime SubmittedAt { get; set; }

        public string Status { get; set; }
    }
}
```

### AI Assistant Advantages

The AI assistant automatically:

✅ **Follows RESTful conventions** (resource URLs, appropriate HTTP verbs)

✅ **Uses proper authentication** ([Authorize] attribute with JWT)

✅ **Includes API versioning** (/api/v1/)

✅ **Returns objects with metadata**, not raw arrays

✅ **Uses ISO 8601 datetime format** (DateTime type serializes correctly)

✅ **Implements pagination** (page, pageSize parameters)

✅ **Handles errors properly** (abstracts technical details, uses HTTP status codes)

✅ **Logs without PII** (structured logging with safe parameters)

✅ **Includes XML documentation** with compliance references

✅ **Uses consistent response formats** (data + meta pattern)

This is what "standards-compliant by default" looks like when AI assistants understand the Government of Canada Standards on APIs.

## Practical Example: Building a Weather-Aware Service

Let's walk through a complete example: building a service that integrates Environment Canada's weather API to determine if outdoor maintenance can be scheduled.

### User Request

"Build a service that checks if outdoor maintenance can be scheduled based on the weather forecast for a given location."

### AI Assistant Workflow

**Step 1: Discover the API**

```
AI: "I found Environment Canada's MSC GeoMet API at api.weather.gc.ca.
It's free, requires no authentication, and provides weather forecasts
with OpenAPI documentation. This is ideal for your use case.

I'll generate a client following GC Standards on APIs..."
```

**Step 2: Generate the Weather Client**

```csharp
using System.Net.Http.Json;

namespace GC.EnvironmentCanada
{
    /// <summary>
    /// Client for Environment and Climate Change Canada's MSC GeoMet API.
    /// Provides weather forecasts and observations for Canadian locations.
    ///
    /// API Documentation: https://api.weather.gc.ca/openapi
    /// No authentication required for public access.
    /// </summary>
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherService> _logger;
        private const string BaseUrl = "https://api.weather.gc.ca";

        public WeatherService(
            HttpClient httpClient,
            ILogger<WeatherService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Get weather forecast for a location.
        /// Uses MSC GeoMet API (GeoJSON format).
        /// </summary>
        /// <param name="latitude">Latitude in decimal degrees (-90 to 90)</param>
        /// <param name="longitude">Longitude in decimal degrees (-180 to 180)</param>
        /// <param name="language">Language for forecast text ("en" or "fr")</param>
        /// <returns>Weather forecast for the location</returns>
        public async Task<WeatherForecast> GetForecastAsync(
            double latitude,
            double longitude,
            string language = "en")
        {
            // Validate inputs
            if (latitude < -90 || latitude > 90)
                throw new ArgumentException("Latitude must be between -90 and 90", nameof(latitude));

            if (longitude < -180 || longitude > 180)
                throw new ArgumentException("Longitude must be between -180 and 180", nameof(longitude));

            if (language != "en" && language != "fr")
                throw new ArgumentException("Language must be 'en' or 'fr'", nameof(language));

            try
            {
                _logger.LogInformation(
                    "Requesting weather forecast for location: {Latitude}, {Longitude}, language: {Language}",
                    latitude,
                    longitude,
                    language
                );

                var url = $"{BaseUrl}/collections/weather-forecast/items" +
                          $"?lat={latitude}&lon={longitude}&lang={language}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var geoJson = await response.Content.ReadFromJsonAsync<GeoJsonFeatureCollection>();

                return ParseForecast(geoJson);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to retrieve weather forecast");
                throw new ExternalServiceException(
                    "Unable to retrieve weather forecast. Please try again later.",
                    ex
                );
            }
        }

        /// <summary>
        /// Determine if weather conditions are suitable for outdoor work.
        /// Business rules:
        ///   - No precipitation > 5mm
        ///   - Wind speed < 40 km/h
        ///   - Temperature > -15°C
        /// </summary>
        public bool IsSuitableForOutdoorWork(WeatherForecast forecast)
        {
            if (forecast == null)
                return false;

            bool suitable = forecast.PrecipitationMm < 5 &&
                           forecast.WindSpeedKmh < 40 &&
                           forecast.TemperatureCelsius > -15;

            _logger.LogInformation(
                "Outdoor work suitability: {Suitable}, Precip: {Precip}mm, Wind: {Wind}km/h, Temp: {Temp}°C",
                suitable,
                forecast.PrecipitationMm,
                forecast.WindSpeedKmh,
                forecast.TemperatureCelsius
            );

            return suitable;
        }

        private WeatherForecast ParseForecast(GeoJsonFeatureCollection geoJson)
        {
            // Parse GeoJSON response into simplified forecast model
            var properties = geoJson.Features.FirstOrDefault()?.Properties;

            if (properties == null)
                throw new ExternalServiceException("Invalid weather data format");

            return new WeatherForecast
            {
                TemperatureCelsius = properties.Temperature,
                PrecipitationMm = properties.Precipitation,
                WindSpeedKmh = properties.WindSpeed,
                ForecastTime = properties.ForecastTime
            };
        }
    }

    // GeoJSON models (Environment Canada uses GeoJSON format)
    public class GeoJsonFeatureCollection
    {
        public List<GeoJsonFeature> Features { get; set; }
    }

    public class GeoJsonFeature
    {
        public WeatherProperties Properties { get; set; }
    }

    public class WeatherProperties
    {
        public double Temperature { get; set; }
        public double Precipitation { get; set; }
        public double WindSpeed { get; set; }
        public DateTime ForecastTime { get; set; }
    }

    // Simplified forecast model
    public class WeatherForecast
    {
        public double TemperatureCelsius { get; set; }
        public double PrecipitationMm { get; set; }
        public double WindSpeedKmh { get; set; }
        public DateTime ForecastTime { get; set; }
    }
}
```

**Step 3: Generate the API Controller**

```csharp
using Microsoft.AspNetCore.Mvc;

namespace GC.MaintenanceScheduler.Controllers
{
    /// <summary>
    /// Weather API Controller.
    /// Provides outdoor work scheduling recommendations based on Environment Canada forecasts.
    ///
    /// GC Compliance:
    ///     - Standards on APIs: RESTful, JSON responses, ISO 8601 dates
    ///     - Official Languages Act: Bilingual responses
    ///     - Data Source: MSC GeoMet (api.weather.gc.ca)
    /// </summary>
    [ApiController]
    [Route("api/v1/weather")]
    [Produces("application/json")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(
            WeatherService weatherService,
            ILogger<WeatherController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        /// <summary>
        /// Get outdoor work suitability for a location and date.
        /// </summary>
        /// <param name="latitude">Location latitude (-90 to 90)</param>
        /// <param name="longitude">Location longitude (-180 to 180)</param>
        /// <param name="language">Response language (en or fr)</param>
        /// <returns>Suitability assessment with weather details</returns>
        /// <response code="200">Returns work suitability assessment</response>
        /// <response code="400">Invalid coordinates or date</response>
        [HttpGet("work-suitability")]
        [ProducesResponseType(typeof(WorkSuitabilityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WorkSuitabilityDto>> GetWorkSuitability(
            [FromQuery] double latitude,
            [FromQuery] double longitude,
            [FromQuery] string language = "en")
        {
            // Get forecast from Environment Canada
            var forecast = await _weatherService.GetForecastAsync(latitude, longitude, language);

            // Determine suitability
            bool suitable = _weatherService.IsSuitableForOutdoorWork(forecast);

            // Build bilingual response
            return Ok(new WorkSuitabilityDto
            {
                Suitable = suitable,
                RecommendationEn = suitable
                    ? "Weather conditions are suitable for outdoor work."
                    : "Weather conditions are not suitable for outdoor work. Reschedule if possible.",
                RecommendationFr = suitable
                    ? "Les conditions météorologiques conviennent au travail extérieur."
                    : "Les conditions météorologiques ne conviennent pas au travail extérieur. Reprogrammez si possible.",
                Weather = new WeatherDetailsDto
                {
                    TemperatureCelsius = forecast.TemperatureCelsius,
                    PrecipitationMm = forecast.PrecipitationMm,
                    WindSpeedKmh = forecast.WindSpeedKmh,
                    ForecastTime = forecast.ForecastTime
                },
                Thresholds = new ThresholdsDto
                {
                    MaxPrecipitationMm = 5,
                    MaxWindSpeedKmh = 40,
                    MinTemperatureCelsius = -15
                }
            });
        }
    }

    // Response DTOs
    public class WorkSuitabilityDto
    {
        public bool Suitable { get; set; }

        // ✅ GOOD: Bilingual recommendations
        public string RecommendationEn { get; set; }
        public string RecommendationFr { get; set; }

        public WeatherDetailsDto Weather { get; set; }
        public ThresholdsDto Thresholds { get; set; }
    }

    public class WeatherDetailsDto
    {
        public double TemperatureCelsius { get; set; }
        public double PrecipitationMm { get; set; }
        public double WindSpeedKmh { get; set; }

        // ✅ GOOD: ISO 8601 datetime
        public DateTime ForecastTime { get; set; }
    }

    public class ThresholdsDto
    {
        public double MaxPrecipitationMm { get; set; }
        public double MaxWindSpeedKmh { get; set; }
        public double MinTemperatureCelsius { get; set; }
    }
}
```

### What the AI Assistant Delivered

In this complete example, the AI assistant:

✅ **Discovered the appropriate API** (MSC GeoMet) for weather data

✅ **Generated a standards-compliant client** with proper error handling

✅ **Implemented business logic** (weather suitability rules)

✅ **Created a RESTful API controller** following Standards on APIs

✅ **Added bilingual responses** (English and French recommendations)

✅ **Used ISO 8601 datetime format** throughout

✅ **Included structured logging** with no PII

✅ **Provided XML documentation** with compliance references

This is a production-ready service built in minutes instead of hours.

## Advanced Patterns: Multi-API Integration

Real applications often need data from multiple government APIs. AI assistants can orchestrate complex integrations automatically.

### The Challenge

Imagine building a demographic analysis tool that needs:

1. **Census data** from Statistics Canada
2. **Geographic boundaries** from GeoGratis (postal code to census subdivision mapping)
3. **Employment statistics** from Open Government Portal

Manually integrating these three sources means:
- Understanding three different API patterns
- Handling three different data formats (JSON, GeoJSON, CSV)
- Coordinating requests and combining responses
- Managing caching to avoid redundant calls
- Handling failures gracefully

### The AI-Assisted Solution

An AI assistant can generate a service that integrates all three sources:

```csharp
using System.Net.Http.Json;

namespace GC.DemographicAnalysis
{
    /// <summary>
    /// Service for demographic analysis combining multiple GC data sources.
    /// Integrates: Statistics Canada Census, GeoGratis boundaries, Open Data employment stats.
    /// </summary>
    public class DemographicAnalysisService
    {
        private readonly OpenDataSearchService _openData;
        private readonly StatCanWdsClient _statCan;
        private readonly GeoGratisClient _geoGratis;
        private readonly IMemoryCache _cache;
        private readonly ILogger<DemographicAnalysisService> _logger;

        public DemographicAnalysisService(
            OpenDataSearchService openData,
            StatCanWdsClient statCan,
            GeoGratisClient geoGratis,
            IMemoryCache cache,
            ILogger<DemographicAnalysisService> logger)
        {
            _openData = openData;
            _statCan = statCan;
            _geoGratis = geoGratis;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Get demographic profile for a postal code.
        /// Combines data from GeoGratis (boundaries), StatCan (census), and Open Data (employment).
        /// </summary>
        /// <param name="postalCode">Canadian postal code (e.g., "K1A 0B1")</param>
        /// <param name="language">Language for metadata ("en" or "fr")</param>
        /// <returns>Demographic profile with population, age, employment data</returns>
        public async Task<DemographicProfile> GetProfileByPostalCodeAsync(
            string postalCode,
            string language = "en")
        {
            // Validate postal code format
            if (!IsValidPostalCode(postalCode))
                throw new ArgumentException("Invalid postal code format", nameof(postalCode));

            _logger.LogInformation(
                "Retrieving demographic profile for postal code: {PostalCode}",
                postalCode
            );

            // Step 1: Convert postal code to geographic boundary (GeoGratis)
            var boundary = await GetBoundaryWithCacheAsync(postalCode);

            // Step 2: Get census data for that boundary (Statistics Canada)
            var censusData = await GetCensusDataWithCacheAsync(
                boundary.CensusSubdivisionId,
                language
            );

            // Step 3: Get employment data (Open Government Portal)
            var employmentData = await GetEmploymentDataWithCacheAsync(
                boundary.Province,
                language
            );

            // Combine and return
            return new DemographicProfile
            {
                PostalCode = postalCode,
                CensusSubdivision = boundary.Name,
                Province = boundary.Province,
                Population = censusData.Population,
                MedianAge = censusData.MedianAge,
                MedianIncome = censusData.MedianIncome,
                UnemploymentRate = employmentData.UnemploymentRate,
                Language = language,
                DataSources = new[]
                {
                    "Statistics Canada Census 2021",
                    "GeoGratis Postal Code Boundaries",
                    "Open Government Employment Statistics"
                },
                LastUpdated = DateTime.UtcNow
            };
        }

        private async Task<GeographicBoundary> GetBoundaryWithCacheAsync(string postalCode)
        {
            var cacheKey = $"boundary:{postalCode}";

            if (_cache.TryGetValue(cacheKey, out GeographicBoundary cached))
            {
                _logger.LogDebug("Cache hit for boundary: {PostalCode}", postalCode);
                return cached;
            }

            _logger.LogDebug("Cache miss for boundary: {PostalCode}, querying GeoGratis", postalCode);
            var boundary = await _geoGratis.GetBoundaryByPostalCodeAsync(postalCode);

            // Cache for 24 hours (boundaries change infrequently)
            _cache.Set(cacheKey, boundary, TimeSpan.FromHours(24));

            return boundary;
        }

        private async Task<CensusData> GetCensusDataWithCacheAsync(
            string censusSdId,
            string language)
        {
            var cacheKey = $"census:{censusSdId}:{language}";

            if (_cache.TryGetValue(cacheKey, out CensusData cached))
            {
                _logger.LogDebug("Cache hit for census data: {CensusSdId}", censusSdId);
                return cached;
            }

            _logger.LogDebug("Cache miss for census data: {CensusSdId}, querying StatCan", censusSdId);
            var censusData = await _statCan.GetCensusDataAsync(censusSdId, language);

            // Cache for 6 months (census updates infrequently)
            _cache.Set(cacheKey, censusData, TimeSpan.FromDays(180));

            return censusData;
        }

        private async Task<EmploymentData> GetEmploymentDataWithCacheAsync(
            string province,
            string language)
        {
            var cacheKey = $"employment:{province}:{language}";

            if (_cache.TryGetValue(cacheKey, out EmploymentData cached))
            {
                _logger.LogDebug("Cache hit for employment data: {Province}", province);
                return cached;
            }

            _logger.LogDebug("Cache miss for employment data: {Province}, querying Open Data", province);
            var employmentData = await _openData.GetEmploymentDataAsync(province, language);

            // Cache for 1 month (employment stats update monthly)
            _cache.Set(cacheKey, employmentData, TimeSpan.FromDays(30));

            return employmentData;
        }

        private bool IsValidPostalCode(string postalCode)
        {
            // Canadian postal code: A1A 1A1 format
            var pattern = @"^[A-Za-z]\d[A-Za-z]\s?\d[A-Za-z]\d$";
            return System.Text.RegularExpressions.Regex.IsMatch(postalCode, pattern);
        }
    }

    // Model classes
    public class DemographicProfile
    {
        public string PostalCode { get; set; }
        public string CensusSubdivision { get; set; }
        public string Province { get; set; }
        public int Population { get; set; }
        public double MedianAge { get; set; }
        public decimal MedianIncome { get; set; }
        public double UnemploymentRate { get; set; }
        public string Language { get; set; }
        public string[] DataSources { get; set; }

        // ✅ GOOD: ISO 8601 datetime
        public DateTime LastUpdated { get; set; }
    }

    public class GeographicBoundary
    {
        public string PostalCode { get; set; }
        public string CensusSubdivisionId { get; set; }
        public string Name { get; set; }
        public string Province { get; set; }
    }

    public class CensusData
    {
        public int Population { get; set; }
        public double MedianAge { get; set; }
        public decimal MedianIncome { get; set; }
    }

    public class EmploymentData
    {
        public double UnemploymentRate { get; set; }
    }
}
```

### What the AI Assistant Added

Notice the advanced patterns:

✅ **Multi-API orchestration** (GeoGratis → StatCan → Open Data)

✅ **Intelligent caching** with different TTLs based on update frequency:
   - Boundaries: 24 hours (change rarely)
   - Census: 6 months (updates every few years)
   - Employment: 1 month (updates monthly)

✅ **Error handling for each service** (failures are isolated)

✅ **Structured logging** with cache hit/miss tracking

✅ **Input validation** (postal code format regex)

✅ **Bilingual data handling** across all sources

✅ **Data source attribution** (transparency about data origins)

✅ **ISO 8601 timestamps** for data freshness

This is production-grade integration code—generated by an AI assistant in minutes.

## Instruction Files for Open Government APIs

Just as we created instruction files for WET-BOEW and the GC Design System in the [first blog post](/blog/government-platforms/canadian-ai-assistants/), we can create them for API integration.

### Proposed Structure

```
.github/
├── instructions/
│   ├── open-government-apis.instructions.md
│   ├── statcan-wds.instructions.md
│   ├── geogratis.instructions.md
│   └── standards-on-apis.instructions.md
└── skills/
    └── api-discovery.md
```

### Example: open-government-apis.instructions.md

Here's what an instruction file for Open Government API integration might look like:

````markdown
---
applyTo: "**/*Service.cs,**/*Client.cs,**/Controllers/**"
---

# Open Government API Integration Instructions

## Discovery Pattern

When asked to find government datasets, query the CKAN API:

```csharp
// Always start with discovery
var searchUrl = "https://open.canada.ca/data/en/api/3/action/package_search";
var query = $"{searchUrl}?q={Uri.EscapeDataString(searchTerm)}&rows=20";
```

## Standards on APIs Compliance

All generated API clients must follow GC Standards on APIs:

### REST Principles
- Resource-oriented URLs (nouns, not verbs)
- Appropriate HTTP verbs (GET, POST, PUT, DELETE)
- JSON request/response bodies (UTF-8 encoding)

### Security
```csharp
// TLS 1.2+ required (configured in HttpClient)
_httpClient.BaseAddress = new Uri("https://api.example.gc.ca");

// JWT Bearer authentication for protected endpoints
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);

// NEVER put API keys in URLs
// ❌ BAD: GET /api/data?apiKey=secret
// ✅ GOOD: Authorization: Bearer <token>
```

### Error Handling
```csharp
try
{
    response.EnsureSuccessStatusCode();
}
catch (HttpRequestException ex)
{
    // Abstract internal details per Standards on APIs
    _logger.LogError(ex, "External API request failed");
    throw new ExternalServiceException(
        "Service temporarily unavailable. Please try again later.",
        ex
    );
}
```

### Date/Time Format
```csharp
// ISO 8601 in UTC (Standards on APIs requirement)
public class ApiResponse
{
    [JsonPropertyName("last_modified")]
    public DateTime LastModified { get; set; }  // Serializes as "2026-02-12T14:30:00Z"
}
```

### Bilingual Support
```csharp
// Official Languages Act compliance
public async Task<DatasetMetadata> GetDatasetAsync(
    string datasetId,
    string language = "en")
{
    var url = $"{BaseUrl}/package_show?id={datasetId}&lang={language}";
    // Returns metadata in requested language
}
```

## Statistics Canada WDS Integration

### Vector Identifiers
StatCan uses vector identifiers (e.g., "v2062815") to reference specific time series.

```csharp
// Labour force, employed, Canada (monthly)
var vectorIds = new List<string> { "v2062815", "v2062816" };
var data = await _statCanClient.GetVectorDataAsync(vectorIds, periods: 60);
```

### SDMX Format Handling
Statistics Canada provides SDMX (Statistical Data and Metadata Exchange) format.

```csharp
// Prefer JSON for simpler integration
var jsonData = await _client.GetDataAsync(productId, format: "json");

// Use SDMX only if required by business needs
var sdmxData = await _client.GetFullTableDownloadSDMXAsync(productId);
```

## GeoGratis Spatial Queries

### Bounding Box Format
```csharp
// west,south,east,north in decimal degrees
var bbox = "-79.5,43.5,-79.3,43.7";  // Toronto area
var results = await _geoGratis.SearchAsync(query: "parks", bbox: bbox);
```

### Response Format Selection
```csharp
// GeoGratis supports multiple formats
var geojson = await _client.GetFeaturesAsync(query, format: "geojson");
var kml = await _client.GetFeaturesAsync(query, format: "kml");
```

## Caching and Performance

Government APIs should be cached to reduce load and improve performance.

```csharp
public async Task<Dataset> GetDatasetWithCacheAsync(string datasetId)
{
    var cacheKey = $"dataset:{datasetId}";

    var cached = await _cache.GetStringAsync(cacheKey);
    if (cached != null)
        return JsonSerializer.Deserialize<Dataset>(cached);

    var dataset = await _openDataClient.GetDatasetAsync(datasetId);

    // Cache for 1 hour (government data updates infrequently)
    await _cache.SetStringAsync(
        cacheKey,
        JsonSerializer.Serialize(dataset),
        new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        }
    );

    return dataset;
}
```

## Logging and Monitoring

```csharp
// ✅ GOOD: Structured logging, no PII
_logger.LogInformation(
    "Retrieved {DatasetCount} datasets for query '{Query}'",
    results.Count,
    searchTerm
);

// ✅ GOOD: Track API performance
using var operation = _telemetry.StartOperation<DependencyTelemetry>("OpenDataAPI");
operation.Telemetry.Data = searchUrl;
var response = await _httpClient.GetAsync(searchUrl);
operation.Telemetry.Success = response.IsSuccessStatusCode;
```
````

### AI Assistant Behavior with Instruction Files

With this instruction file in place, the AI assistant will automatically:

✅ **Follow Standards on APIs** when generating integration code

✅ **Include bilingual support** by default (language parameters)

✅ **Add proper error handling** (abstracted technical details)

✅ **Use caching** for government API calls

✅ **Validate data formats** (ISO 8601 dates, proper JSON structure)

✅ **Include structured logging** with performance tracking

✅ **Handle government-specific formats** (SDMX, GeoJSON, CKAN)

This turns the Standards on APIs from a document you have to remember into patterns that are applied automatically.

## Real-World Benefits

What does this mean in practical terms for developers and contractors?

### For Government Contractors

**1. Faster Dataset Discovery**

Traditional approach: Hours of manual browsing on open.canada.ca

AI-assisted approach: "Find census data for Ontario municipalities" → AI queries CKAN, presents ranked options in seconds

**Time saved**: 2-4 hours per dataset discovery task

**2. Automatic Compliance**

Traditional approach: Read Standards on APIs, manually ensure compliance, fail reviews, fix issues

AI-assisted approach: Generated code follows Standards on APIs automatically (TLS 1.2+, JWT, ISO 8601, proper error handling)

**Time saved**: 1-2 days per API integration (no rework cycles)

**3. Reduced Boilerplate**

Traditional approach: Write HTTP clients, DTOs, error handling, logging from scratch

AI-assisted approach: AI generates complete clients with proper patterns in minutes

**Time saved**: 4-6 hours per API client

**4. Better Documentation**

Traditional approach: Minimal code comments, no compliance references

AI-assisted approach: XML comments reference official docs, include compliance citations

**Quality improvement**: Code reviews pass faster, maintenance is easier

**5. Cross-API Integration**

Traditional approach: Manually coordinate multiple APIs, handle format conversions, manage caching

AI-assisted approach: AI orchestrates complex integrations (CKAN + StatCan + GeoGratis) automatically

**Time saved**: 1-2 days for multi-API services

### For Government Departments

**1. Increased API Adoption**

Lower barrier to integration → More developers use official APIs instead of downloading static files

**2. Better API Compliance**

Developers generate standards-compliant code automatically → Fewer support requests, better API usage patterns

**3. Improved Data Quality**

AI can validate data freshness, flag deprecated datasets, suggest authoritative sources

## Practical Steps: What You Can Do Today

### For Developers

**1. Add Open Government API instruction files** to your project:

Create `.github/instructions/open-government-apis.instructions.md` with CKAN API patterns and Standards on APIs compliance rules.

**2. Use natural language discovery**:

Ask your AI assistant: "Find datasets about employment by province"

Let AI query CKAN and present options

**3. Generate standards-compliant clients**:

Request: "Generate a client for Statistics Canada labour force data"

Review generated code for Standards on APIs compliance

**4. Create reusable API clients**:

Build a shared library of government API clients

Share across your organization's projects

### For Organizations

**1. Create canonical instruction files**:

Official patterns for CKAN, StatCan WDS, GeoGratis integration

Standards on APIs compliance templates

Share via internal repositories or GCcode

**2. Build API client libraries**:

.NET packages for common government APIs

Published to NuGet or internal package feeds

AI-assisted maintenance and updates

**3. Contribute to documentation**:

Improve open.canada.ca API documentation

Add more examples for Standards on APIs

Share integration patterns with community

## Caveats and Considerations

### Data Quality and Timeliness

**AI can find datasets but can't validate data quality**

Always check:
- "Last updated" dates
- Data sources (prefer authoritative departments)
- Update frequency for your use case
- Data completeness and coverage

### API Limitations

**No authentication = no rate limiting visibility**

Be aware:
- open.canada.ca only supports GET requests
- Some APIs lack comprehensive documentation
- Not all departments provide APIs (some only CSV downloads)

### Standards Compliance

**AI-generated code should be reviewed**

Validate:
- Security practices (TLS, JWT, no credentials in code)
- Error handling and edge cases
- Bilingual support actually works correctly
- Performance implications (caching, pagination)

### Dependency on External Services

**Government APIs can have outages**

Plan for:
- Service degradation and fallback strategies
- Proper caching to reduce dependency
- Monitoring API health and performance
- Alternative data sources if primary fails

## Conclusion: AI Assistants as Data Integration Partners

Canada's Open Government API ecosystem isn't just for manual integration—it's a machine-readable catalog that AI assistants can query, understand, and integrate automatically.

**The opportunity**:
- Developers can ask AI to find relevant datasets (discovery in seconds, not hours)
- AI generates standards-compliant integration code (compliance by default)
- Bilingual support is built-in automatically
- Multi-API orchestration happens seamlessly

**The path forward**:

**For developers**: Try natural language dataset discovery on your next project. Generate a Statistics Canada WDS client. Review the code against Standards on APIs. Iterate and improve.

**For organizations**: Create instruction files for government API integration. Share patterns across teams. Contribute to improving government API documentation.

**For government**: The combination of Canada's comprehensive Open Government APIs and AI coding assistants creates a powerful development environment. When AI assistants can discover datasets, generate compliant integration code, and handle government-specific requirements automatically, developers can focus on building services that actually help Canadians—not wrestling with API documentation.

---

## Your Turn

Have you integrated Canadian open data into your applications? What APIs do you use most? Have you tried using AI assistants for dataset discovery or API client generation?

I'd love to hear your experiences. Drop a comment below.

---

*This post builds on the insights from [How Canadian Government Platforms Make AI Coding Assistants More Effective](/blog/government-platforms/canadian-ai-assistants/), exploring AI assistants as active consumers of government data rather than passive followers of coding patterns.*

**Key Resources**:
- [Government of Canada Standards on APIs](https://www.canada.ca/en/government/system/digital-government/digital-government-innovations/government-canada-standards-apis.html)
- [Open Government Portal API](https://open.canada.ca/en/access-our-application-programming-interface-api)
- [Statistics Canada Web Data Service](https://www.statcan.gc.ca/en/developers/wds)
- [GeoGratis API Documentation](https://geogratis.gc.ca/site/eng/api/documentation/sguide)
- [MSC GeoMet Weather API](https://api.weather.gc.ca/)
