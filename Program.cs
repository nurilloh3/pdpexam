using System;
using pdpExam4.Brokers.Storage;
using pdpExam4.Models;

namespace pdpExam4;

class Program
{
    static public StorageBroker storageBroker = new StorageBroker("localhost", "Event_zone", "sa", "Nurilloh0850@123456");
    static public User user = null;
    static async Task Main(string[] args)
    {
        while (true)
        {

            if (user is null) await signMenu();
            else if (user.Role == 0) await MenuForAdmin();
            else if (user.Role == 1) await MenuForCompany();
        }
        
    }

    static async Task signMenu()
    {
        Console.WriteLine("1. Kirish");
        Console.WriteLine("2. Royxatdan otish");
        int input = int.Parse(Console.ReadLine());
        Console.Clear();
        if (input == 1)
            await signin();


    }
    static async Task signin()
    {
        string login = default;
        string password = default;

        Console.WriteLine("Login Kiriting:");
        login = Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Parol kiriting: ");
        password = Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Kirish amalga oshirilyapti");
        user = await storageBroker.Login(login, password);
        Console.Clear();
        if (user is null)
            Console.WriteLine("Parol yoki login xato");
    }

    static async Task signup(string name, string login, string password)
    {
        Console.WriteLine("Ismingizni kiriting: ");
        name = Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Login kiriting: ");
        login = Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Parol kiriting: ");
        password = Console.ReadLine();
        Console.Clear();
        Console.WriteLine("Royxatdan otish amalga oshirilyapti");
        user = await storageBroker.Register(name, login, password);
        Console.Clear();
    }

    static async Task MenuForAdmin()
    {
        while (true)
        {
            Console.WriteLine("1. Hamma buyurtmalarni korish");
            Console.WriteLine("2. Yangi buyurtmalarni korish");
            Console.WriteLine("3. Active Buyurtmalarni korish");
            Console.WriteLine("Chiqish uchun 0 ni kiriting");

            int input = int.Parse(Console.ReadLine());
            Console.Clear();
            if (input == 0)
            {
                user = null;
                return;
            }
            else if (input > 0 && input < 4)
            {
                Console.WriteLine("Ortga chiqish uchun 0 ni kiriting");
                var applications = await storageBroker.GetBookings((StorageBroker.GetBookingType)input);
                for (int i = 0; i < applications.Count(); i++)
                {
                    Console.WriteLine($"{applications[i].RoomId} - xonaga buyurtma, statusi: {(StorageBroker.BookingStatus)applications[i].Status}");
                }

                Console.Write("Buyutma raqamini tanlang: ");
                input = int.Parse(Console.ReadLine());
                Console.Clear();
                if (input == 0)
                {
                    continue;
                }

                EditApplication(applications[input-1]);
            }
        }

        static async Task EditApplication(Application application)
        {
            Console.WriteLine("1. Qabul qilindi");
            Console.WriteLine("2. Bekor qilindi");
            Console.WriteLine("0. Ortga qaytish uchun 0 ni kiriting");
            int input = int.Parse(Console.ReadLine());
            Console.Clear();
            if (input == 0) return;
            if(input == 1)
            {
                string message = $"Sizning {application.Id} sonli buyutma adminstrator tamonidan qabul qilindi";
                Console.WriteLine("Bajarilmoqda...");
                await storageBroker.EditStatusApplication(application.Id, message, (int)StorageBroker.BookingStatus.Bajarilgan, application.FromUser);
                await storageBroker.SetBusyStatusToRoom(application.RoomId, application.AtTime, application.ToTime);
            }
            else if(input == 2)
            {
                string message = $"Sizning {application.Id} sonli buyutma adminstrator tamonidan bekor qilindi";
                Console.WriteLine("Bajarilmoqda...");
                await storageBroker.EditStatusApplication(application.Id, message, (int)StorageBroker.BookingStatus.Bekor, application.FromUser);
            }
            Console.Clear();
            Console.WriteLine("Status ozgartirildi");
        }
        
    }

    static async Task MenuForCompany()
    {
        Console.WriteLine("1. Bosh xonalar");
        Console.WriteLine("2. Band qilingan xonalar");
        int input = int.Parse(Console.ReadLine());
        if(input == 1)
        {

        }
    }


}
