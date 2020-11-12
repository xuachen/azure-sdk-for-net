// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.Security.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// A vulnerability assessment scan record properties.
    /// </summary>
    public partial class ScanProperties
    {
        /// <summary>
        /// Initializes a new instance of the ScanProperties class.
        /// </summary>
        public ScanProperties()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ScanProperties class.
        /// </summary>
        /// <param name="triggerType">Possible values include: 'OnDemand',
        /// 'Recurring'</param>
        /// <param name="state">Possible values include: 'Failed',
        /// 'FailedToRun', 'InProgress', 'Passed'</param>
        /// <param name="server">The server name.</param>
        /// <param name="database">The database name.</param>
        /// <param name="sqlVersion">The SQL version.</param>
        /// <param name="startTime">The scan start time (UTC).</param>
        /// <param name="endTime">Scan results are valid until end time
        /// (UTC).</param>
        /// <param name="highSeverityFailedRulesCount">The number of failed
        /// rules with high severity.</param>
        /// <param name="mediumSeverityFailedRulesCount">The number of failed
        /// rules with medium severity.</param>
        /// <param name="lowSeverityFailedRulesCount">The number of failed
        /// rules with low severity.</param>
        /// <param name="totalPassedRulesCount">The number of total passed
        /// rules.</param>
        /// <param name="totalFailedRulesCount">The number of total failed
        /// rules.</param>
        /// <param name="totalRulesCount">The number of total rules
        /// assessed.</param>
        /// <param name="isBaselineApplied">Baseline created for this database,
        /// and has one or more rules.</param>
        public ScanProperties(string triggerType = default(string), string state = default(string), string server = default(string), string database = default(string), string sqlVersion = default(string), System.DateTime? startTime = default(System.DateTime?), System.DateTime? endTime = default(System.DateTime?), int? highSeverityFailedRulesCount = default(int?), int? mediumSeverityFailedRulesCount = default(int?), int? lowSeverityFailedRulesCount = default(int?), int? totalPassedRulesCount = default(int?), int? totalFailedRulesCount = default(int?), int? totalRulesCount = default(int?), bool? isBaselineApplied = default(bool?))
        {
            TriggerType = triggerType;
            State = state;
            Server = server;
            Database = database;
            SqlVersion = sqlVersion;
            StartTime = startTime;
            EndTime = endTime;
            HighSeverityFailedRulesCount = highSeverityFailedRulesCount;
            MediumSeverityFailedRulesCount = mediumSeverityFailedRulesCount;
            LowSeverityFailedRulesCount = lowSeverityFailedRulesCount;
            TotalPassedRulesCount = totalPassedRulesCount;
            TotalFailedRulesCount = totalFailedRulesCount;
            TotalRulesCount = totalRulesCount;
            IsBaselineApplied = isBaselineApplied;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets possible values include: 'OnDemand', 'Recurring'
        /// </summary>
        [JsonProperty(PropertyName = "triggerType")]
        public string TriggerType { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Failed', 'FailedToRun',
        /// 'InProgress', 'Passed'
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the server name.
        /// </summary>
        [JsonProperty(PropertyName = "server")]
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        [JsonProperty(PropertyName = "database")]
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the SQL version.
        /// </summary>
        [JsonProperty(PropertyName = "sqlVersion")]
        public string SqlVersion { get; set; }

        /// <summary>
        /// Gets or sets the scan start time (UTC).
        /// </summary>
        [JsonProperty(PropertyName = "startTime")]
        public System.DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets scan results are valid until end time (UTC).
        /// </summary>
        [JsonProperty(PropertyName = "endTime")]
        public System.DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the number of failed rules with high severity.
        /// </summary>
        [JsonProperty(PropertyName = "highSeverityFailedRulesCount")]
        public int? HighSeverityFailedRulesCount { get; set; }

        /// <summary>
        /// Gets or sets the number of failed rules with medium severity.
        /// </summary>
        [JsonProperty(PropertyName = "mediumSeverityFailedRulesCount")]
        public int? MediumSeverityFailedRulesCount { get; set; }

        /// <summary>
        /// Gets or sets the number of failed rules with low severity.
        /// </summary>
        [JsonProperty(PropertyName = "lowSeverityFailedRulesCount")]
        public int? LowSeverityFailedRulesCount { get; set; }

        /// <summary>
        /// Gets or sets the number of total passed rules.
        /// </summary>
        [JsonProperty(PropertyName = "totalPassedRulesCount")]
        public int? TotalPassedRulesCount { get; set; }

        /// <summary>
        /// Gets or sets the number of total failed rules.
        /// </summary>
        [JsonProperty(PropertyName = "totalFailedRulesCount")]
        public int? TotalFailedRulesCount { get; set; }

        /// <summary>
        /// Gets or sets the number of total rules assessed.
        /// </summary>
        [JsonProperty(PropertyName = "totalRulesCount")]
        public int? TotalRulesCount { get; set; }

        /// <summary>
        /// Gets or sets baseline created for this database, and has one or
        /// more rules.
        /// </summary>
        [JsonProperty(PropertyName = "isBaselineApplied")]
        public bool? IsBaselineApplied { get; set; }

    }
}