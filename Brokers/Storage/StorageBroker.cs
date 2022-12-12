using System;
using System.Data.SqlClient;
using pdpExam4.Models;

namespace pdpExam4.Brokers.Storage
{
	public class StorageBroker : IStorageBroker
	{
        private SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
        public StorageBroker(string host, string database, string userId, string password)
		{
            this.connectionString.DataSource = host;
            this.connectionString.InitialCatalog = database;
            this.connectionString.UserID = userId;
            this.connectionString.Password = password;
            this.connectionString.Pooling = true;
        }

        public SqlConnection GetConnection() => new SqlConnection(connectionString.ConnectionString);

        public async Task BookingPalace(int userid, int palace, int applicationId)
        {
            using SqlConnection connection = GetConnection();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "[dbo].[BookingRoom]";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@userid", userid));
            command.Parameters.Add(new SqlParameter("@place", palace));
            command.Parameters.Add(new SqlParameter("@applicetionid", applicationId));

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }

        public async Task BookingRoom(int userid, DateTime at_time, DateTime to_time, int roomid)
        {
            using SqlConnection connection = GetConnection();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "[dbo].[BookingRoom]";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@userid", userid));
            command.Parameters.Add(new SqlParameter("@at_time", at_time));
            command.Parameters.Add(new SqlParameter("@to_time", to_time));
            command.Parameters.Add(new SqlParameter("@roomid", roomid));

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            

        }

        public async Task<List<Application>> GetMyBooking(int userid)
        {
            using SqlConnection connection = GetConnection();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "[dbo].[MyBookings]";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@userid", userid));

            await connection.OpenAsync();
            SqlDataReader dataReader = await command.ExecuteReaderAsync();
            List<Application> applications = new List<Application>();
            while(await dataReader.ReadAsync())
            {
                applications.Add(new Application(
                    dataReader.GetInt32(0),
                    dataReader.GetInt32(1),
                    dataReader.GetDateTime(2),
                    dataReader.GetDateTime(3),
                    dataReader.GetInt32(4),
                    dataReader.GetInt32(5)));
            }
            await connection.CloseAsync();
            return applications;

        }

        public async Task<User> Login(string login, string password)
        {
            using SqlConnection connection = GetConnection();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "[dbo].[Login]";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@login", login));
            command.Parameters.Add(new SqlParameter("@password", password));

            await connection.OpenAsync();
            SqlDataReader dataReader = await command.ExecuteReaderAsync();
            User user = null;
            while (await dataReader.ReadAsync())
            {
                user = new User(
                    dataReader.GetInt32(0),
                    dataReader.GetString(1),
                    dataReader.GetString(2),
                    dataReader.GetString(3),
                    dataReader.GetInt32(4));
            }
            await connection.CloseAsync();
            return user;
        }

        public async Task<User> Register(string name, string login, string password)
        {
            using SqlConnection connection = GetConnection();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "[dbo].[Register]";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@login", login));
            command.Parameters.Add(new SqlParameter("@password", password));
            command.Parameters.Add(new SqlParameter("@name", name));

            await connection.OpenAsync();
            SqlDataReader dataReader = await command.ExecuteReaderAsync();
            User user = null;
            while (await dataReader.ReadAsync())
            {
                user = new User(
                    dataReader.GetInt32(0),
                    dataReader.GetString(1),
                    dataReader.GetString(2),
                    dataReader.GetString(3),
                    dataReader.GetInt32(4));
            }
            await connection.CloseAsync();
            return user;
        }

        public async Task<List<Application>> GetBookings(GetBookingType type)
        {
            string commandText = default;
            if (type == GetBookingType.ActiveBookings) commandText = "[dbo].[GetActiveBookings]";
            else if (type == GetBookingType.AllBookings) commandText = "[dbo].[GetAllBookings]";
            else if (type == GetBookingType.NewBookings) commandText = "[dbo].[GetNewBookings]";
            using SqlConnection connection = GetConnection();
            using SqlCommand sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = commandText;
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            using var dataReader = await sqlCommand.ExecuteReaderAsync();
            List<Application> result = new List<Application>();
            await connection.OpenAsync();
            while(await dataReader.ReadAsync())
            {
                result.Add(new Application(
                    dataReader.GetInt32(0),
                    dataReader.GetInt32(1),
                    dataReader.GetDateTime(2),
                    dataReader.GetDateTime(3),
                    dataReader.GetInt32(4),
                    dataReader.GetInt32(5)));
            }
            await connection.CloseAsync();
            return result;
        }

        public async Task EditStatusApplication(int applicationid, string message, int status, int userId)
        {
            using SqlConnection connection = GetConnection();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "[dbo].[editStatusApplication]";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@userid", userId));
            command.Parameters.Add(new SqlParameter("@applicationid", applicationid));
            command.Parameters.Add(new SqlParameter("@message", message));
            command.Parameters.Add(new SqlParameter("@status", status));

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }

        public async Task SetBusyStatusToRoom(int roomId, DateTime at_date, DateTime to_Date)
        {
            using SqlConnection connection = GetConnection();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "[dbo].[SetBusyToRoom]";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@roomid", roomId));
            command.Parameters.Add(new SqlParameter("@at_date", at_date));
            command.Parameters.Add(new SqlParameter("@to_date", to_Date));

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }

        public enum GetBookingType
        {
            NewBookings = 1,
            AllBookings = 2,
            ActiveBookings = 3
        }
        public enum BookingStatus
        {
            Teshirilmagan,
            Bajarilgan,
            Bekor
        }
    }
}

