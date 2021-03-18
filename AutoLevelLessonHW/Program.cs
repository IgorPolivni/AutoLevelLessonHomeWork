using System;
using System.Data;
using System.Data.SqlClient;

namespace AutoLevelLessonHW
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSet shopDB = new DataSet("ShopDB");
            
            string  connectionString = @"Data Source=DESKTOP-619RM10;Initial Catalog=AutoLevelLessonHW;Integrated Security=True",
                    sqlScript = "";

            SqlDataAdapter adapter = new SqlDataAdapter("select * from People", @"Data Source=DESKTOP-619RM10;Initial Catalog=ConectionLesson;Integrated Security=True");

            adapter.Fill(shopDB);

            //Таблица Заказы
            DataTable TableOrders = new DataTable("Orders");
            //Добавление стоблцов
            TableOrders.Columns.Add(CreateIdColumn());
            TableOrders.Columns.Add("CustomerId", typeof(int));
            //Определение первичного ключа
            TableOrders.PrimaryKey = new DataColumn[] { TableOrders.Columns["Id"] };


            //Таблица Клиенты
            DataTable TableCustomers = new DataTable("Customers");
            //Добавление стоблцов
            TableCustomers.Columns.Add(CreateIdColumn());
            TableCustomers.Columns.Add("Name", typeof(string));
            //Определение первичного ключа
            TableCustomers.PrimaryKey = new DataColumn[] { TableCustomers.Columns["Id"] };

            //Таблица работники
            DataTable TableEmployees = new DataTable("Employees");
            //Добавление стоблцов
            TableEmployees.Columns.Add(CreateIdColumn());
            TableEmployees.Columns.Add("Name", typeof(string));
            TableEmployees.Columns.Add("Position", typeof(string));
            //Определение первичного ключа
            TableEmployees.PrimaryKey = new DataColumn[] { TableEmployees.Columns["Id"] };


            //Таблица Товары
            DataTable TableProducts = new DataTable("Products");
            //Добавление стоблцов
            TableProducts.Columns.Add(CreateIdColumn());
            TableProducts.Columns.Add("Name", typeof(string));
            TableProducts.Columns.Add("Price", typeof(double));
            //Определение первичного ключа
            TableProducts.PrimaryKey = new DataColumn[] { TableProducts.Columns["Id"] };


            //Таблица Товары
            DataTable TableOrderDetails = new DataTable("OrderDetails");
            //Добавление стоблцов
            TableOrderDetails.Columns.Add(CreateIdColumn());
            TableOrderDetails.Columns.Add("OrderId",typeof(int));
            TableOrderDetails.Columns.Add("ProductId", typeof(int));
            //Определение первичного ключа
            TableOrderDetails.PrimaryKey = new DataColumn[] { TableOrderDetails.Columns["Id"] };


            //Добавление таблиц в БД
            shopDB.Tables.Add(TableOrders);
            shopDB.Tables.Add(TableCustomers);
            shopDB.Tables.Add(TableEmployees);
            shopDB.Tables.Add(TableProducts);
            shopDB.Tables.Add(TableOrderDetails);

            //Добавление внешних ключенй
            ForeignKeyConstraint FK_Customers_Orders = new ForeignKeyConstraint("FK_Customers_Orders",
                shopDB.Tables[TableOrders.TableName].Columns["CustomerId"],
                shopDB.Tables[TableCustomers.TableName].Columns["Id"]);

            shopDB.Tables[TableCustomers.TableName].Constraints.Add(FK_Customers_Orders);

            ForeignKeyConstraint FK_OrderDetails_Orders = new ForeignKeyConstraint("FK_OrderDetails_Orders",
                shopDB.Tables[TableOrderDetails.TableName].Columns["OrderId"],
                shopDB.Tables[TableOrders.TableName].Columns["Id"]);

            shopDB.Tables[TableOrders.TableName].Constraints.Add(FK_OrderDetails_Orders);

            ForeignKeyConstraint FK_OrderDetails_Products = new ForeignKeyConstraint("FK_OrderDetails_Products",
                shopDB.Tables[TableOrderDetails.TableName].Columns["ProductId"],
                shopDB.Tables[TableProducts.TableName].Columns["Id"]);

            shopDB.Tables[TableProducts.TableName].Constraints.Add(FK_OrderDetails_Products);

            //Сохранение изменений
            SqlCommandBuilder SqlCommandBuilder = new SqlCommandBuilder(adapter);

            adapter.Update(shopDB);
            shopDB.AcceptChanges();



        }

        private static DataColumn CreateIdColumn()
        {
            DataColumn id = new DataColumn("Id", typeof(int));
            id.AutoIncrement = true;
            id.AutoIncrementSeed = 1;
            id.AutoIncrementStep = 1;
            id.ReadOnly = true;
            return id;
        }
    }
}
