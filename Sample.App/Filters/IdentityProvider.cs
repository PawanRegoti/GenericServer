namespace Sample.App.Filters
{
	public interface IIdentityProvider
	{
		int UserId { get; }
    void SetUserId(int userId);
  }

	public class IdentityProvider : IIdentityProvider
	{
		public int UserId { get; private set; }

		public void SetUserId(int userId)
		{
      UserId = userId;
		}
	}
}
