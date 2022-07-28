using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessTextToSpeech.Common.Model;

public class NotifyPollyCompleteModel
{
    [JsonPropertyName("TaskId")]
    public string TaskId { get; set; }

    [JsonPropertyName("TaskStatus")]
    public string TaskStatus { get; set; }
}
