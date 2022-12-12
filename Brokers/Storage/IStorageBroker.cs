using System;
using System.Data.SqlClient;
using pdpExam4.Models;
using static pdpExam4.Brokers.Storage.StorageBroker;

namespace pdpExam4.Brokers.Storage
{
	public partial interface IStorageBroker
	{
		public Task<User> Register(string name, string login, string password);
		public Task<User> Login(string login, string password);
		public Task BookingRoom(int userid, DateTime at_time, DateTime to_time, int roomid);
		public Task BookingPalace(int userid, int palace, int applicationId);
		public Task<List<Application>> GetMyBooking(int userid);
		public SqlConnection GetConnection();
		public Task<List<Application>> GetBookings(GetBookingType type);
		public Task EditStatusApplication(int applicationid, string message, int status, int userId);
		public Task SetBusyStatusToRoom(int roomId, DateTime at_date, DateTime to_Date);

	}
}

