using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelWebApp.Models;

namespace HotelWebApp.Data
{
    public static class Initializer
    {
        private static Random random = new Random();

        private static string[] names =
        {
            "Никита", "Евгений", "Михаил", "Даниил", "Юрий", "Илья", "Алексей", "Сергей", "Виталий",
            "Денис", "Михаил", "Артем", "Иван", "Виктор", "Александр"
        };

        private static string[] surnames =
        {
            "Кириленко", "Панфилов", "Дубровский", "Долгов", "Яркин", "Черкасов", "Рябцев", "Жилин", "Фокин",
            "Мосин", "Агапов", "Якубовский", "Никулин", "Котов", "Прохоров"
        };

        private static string[] middleNames =
        {
            "Алексеевич", "Дмитриевич", "Владимирович", "Евгеньевич", "Артёмович", "Павловнич", "Николаевнич",
            "Антонович", "Викторович", "Кириллович", "Андреевич", "Геннадьевич", "Даниилович", "Олегович", "Егорович"
        };

        private static string[] lists =
        {
            "Уборка", "Замена белья", "Стирка вещей", "Доставка", "Обед", "Ужин", "Завтрак", "Пользование стоянкой",
            "Услуги тренажерного зала"
        };

        private static string[] types =
        {
            "Одноместный", "Двухместный", "Трехместный", "Четырехместный", "Люкс", "Президентский Люкс"
        }; 
        private static string[] positions =
        {
            "Агент", "Менеджер", "Администратор", "Дворник", "Повар", "Шеф-повар", "Официант"
        };

        private static char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

        private static string GetRandomEl(string[] arr)
        {
            return arr[random.Next(0, arr.Length)];
        }

        private static DateTime NextDateTime()
        {
            DateTime start = new DateTime(2017, 1, 1);
            int range = (DateTime.Today - start).Days;

            return start.AddDays(random.Next(range));
        }

        private static string GetString(int minStringLength, int maxStringLength)
        {
            string result = "";

            int stringLimit = minStringLength + random.Next(maxStringLength - minStringLength);

            int stringPosition;
            for (int i = 0; i < stringLimit; i++)
            {
                stringPosition = random.Next(letters.Length);

                result += letters[stringPosition];
            }

            return result;
        }

        public static void Initialize(HotelContext db)
        {
            int rowCount;
            int rowIndex;

            if (!db.Clients.Any())
            {
                rowCount = 100;
                rowIndex = 0;

                while (rowIndex < rowCount)
                {
                    Client client = new Client
                    {
                        FullName = GetRandomEl(surnames) + " " + GetRandomEl(names) + " " + GetRandomEl(middleNames),
                        PassportData = GetString(8, 8),
                        NameOfRoom = GetString(10, 16),
                        List = GetRandomEl(lists),
                        TotalCost = random.Next(90, 4000)
                    };
                    db.Clients.Add(client);
                    rowIndex++;
                }

                db.SaveChanges();
            }

            if (!db.RoomRates.Any())
            {
                rowIndex = 0;
                rowCount = 100;

                while (rowIndex < rowCount)
                {
                    RoomRate rr = new RoomRate()
                    {
                        Cost = random.Next(400, 1399),
                        Date = NextDateTime()
                    };
                    db.RoomRates.Add(rr);


                    rowIndex++;
                }
                db.SaveChanges();
            }

            if (!db.Employees.Any())
            {
                rowIndex = 0;
                rowCount = 100;

                while (rowIndex < rowCount)
                {
                    Employee emp = new Employee
                    {
                        FullName = GetRandomEl(surnames) + " " + GetRandomEl(names),
                        Position = GetRandomEl(positions)
                    };
                    db.Employees.Add(emp);


                    rowIndex++;
                }

                db.SaveChanges();
            }

            if (!db.Rooms.Any())
            {
                rowCount = 1000;
                rowIndex = 0;

                while (rowIndex < rowCount)
                {
                    int minId = db.RoomRates.Min(x => x.Id);
                    int maxId = db.RoomRates.Max(x => x.Id);

                    Room room = new Room
                    {
                        Type = GetRandomEl(types),
                        Capacity = random.Next(1, 4),
                        Specification = GetString(8, 16),
                        RoomRateId = random.Next(minId, maxId + 1)
                    };
                    db.Rooms.Add(room);
                    rowIndex++;
                }

                db.SaveChanges();
            }

            if (!db.Orders.Any())
            {
                rowIndex = 0;
                rowCount = 1000;

                int minClientId = db.Clients.Min(x => x.Id);
                int maxClientId = db.Clients.Max(x => x.Id);

                int minEmployeeId = db.Employees.Min(x => x.Id);
                int maxEmployeeId = db.Employees.Max(x => x.Id);

                int minRoomId = db.Rooms.Min(x => x.Id);
                int maxRoomId = db.Rooms.Max(x => x.Id);

                while (rowIndex < rowCount)
                {
                    Order order = new Order
                    {
                        CheckInDate = NextDateTime(),
                        CheckOut = NextDateTime(),
                        EmployeeId = random.Next(minEmployeeId, maxEmployeeId + 1),
                        ClientId = random.Next(minClientId, maxClientId + 1),
                        RoomId = random.Next(minRoomId, maxRoomId + 1)
                    };
                    db.Orders.Add(order);
                    rowIndex++;
                }

                db.SaveChanges();

            }

            if (!db.ServiceTypes.Any())
            {
                rowIndex = 0;
                rowCount = 1000;

                while (rowIndex < rowCount)
                {
                    ServiceType serviceType = new ServiceType
                    {
                        Name = GetRandomEl(lists),
                        Specificaion = GetString(8, 12)
                    };
                    db.ServiceTypes.Add(serviceType);


                    rowIndex++;
                }
                db.SaveChanges();
            }



            if (!db.Services.Any())
            {
                rowIndex = 0;
                rowCount = 1000;

                while (rowIndex < rowCount)
                {
                    int minEmployeeId = db.Employees.Min(x => x.Id);
                    int maxEmployeeId = db.Employees.Max(x => x.Id);

                    int minSt = db.ServiceTypes.Min(x => x.Id);
                    int maxSt = db.ServiceTypes.Max(x => x.Id);

                    Service service = new Service()
                    {
                        Cost = random.Next(400, 3213),
                        ServiceTypeId = random.Next(minSt, maxSt+1),
                        EmployeeId = random.Next(minEmployeeId, maxEmployeeId+1)
                    };
                    db.Services.Add(service);


                    rowIndex++;
                }
                db.SaveChanges();
            }
        }
    }
}
