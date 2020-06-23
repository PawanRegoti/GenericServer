using System.Collections.Generic;

namespace Sample.App
{
  public class SampleDto
  {
    public int DocumentNr { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    public IEnumerable<int> PhoneNumbers { get; set; }
  }
}
