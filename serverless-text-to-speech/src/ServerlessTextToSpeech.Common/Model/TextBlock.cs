using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessTextToSpeech.Common.Model;

public class TextBlock
{

    public TextBlock()
    {

    }

    public TextBlock(string text) 
        : this()
    {
        Text = text;
    }
    public string? Text { get; set; }
}
