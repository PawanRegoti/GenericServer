using Sample.Dal;

namespace Sample.App.Mappers
{
  public static class SampleMapper
  {
    public static SampleDto Map(SampleModel sampleModel)
    {
      if (sampleModel == null)
      {
        return null;
      }

      return new SampleDto
      {
        DocumentNr = sampleModel.DocumentNr,
        Name = sampleModel.Name,
        Address = sampleModel.Address,
        City = sampleModel.City,
        Country = sampleModel.Country,
        PhoneNumbers = sampleModel.PhoneNumbers
      };
    }

    public static SampleModel Map(int userId, SampleDto sampleDto)
    {
      if (sampleDto == null)
      {
        return null;
      }

      return new SampleModel(userId, sampleDto.DocumentNr)
      {
        Name = sampleDto.Name,
        Address = sampleDto.Address,
        City = sampleDto.City,
        Country = sampleDto.Country,
        PhoneNumbers = sampleDto.PhoneNumbers
      };
    }
  }
}
