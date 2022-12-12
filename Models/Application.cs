using System;
namespace pdpExam4.Models
{
	public class Application
	{
		public Application(int id, int fromuser, DateTime atTime, DateTime toTime, int roomId, int status)
		{
			this.Id = id;
			this.FromUser = fromuser;
			this.AtTime = atTime;
			this.ToTime = toTime;
			this.Status = status;
		}

		public int Id { get; set; }
		public int FromUser { get; set; }
		public DateTime AtTime { get; set; }
		public DateTime ToTime { get; set; }
		public int RoomId { get; set; }
		public int Status { get; set; }
	}
}

