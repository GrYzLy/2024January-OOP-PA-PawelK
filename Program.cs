using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Client client = new Client("PawelKrygowski", "Heymansstrat64, Rotterdam");

        SavingsAccount savingsAccount = new SavingsAccount(1000, 0.02m);
        InvestmentAccount investmentAccount = new InvestmentAccount(5000, 0.02m);

        client.AddAccount(savingsAccount);
        client.AddAccount(investmentAccount);

        while (true)
        {
            Console.WriteLine("\n--- Menu ---");
            Console.WriteLine("1. Show Savings Account Info");
            Console.WriteLine("2. Show Investment Account Info");
            Console.WriteLine("3. Deposit to Savings Account");
            Console.WriteLine("4. Withdraw from Savings Account");
            Console.WriteLine("5. Apply Interest to Savings Account");
            Console.WriteLine("6. Buy Stock");
            Console.WriteLine("7. Show Stock Orders");
            Console.WriteLine("8. Exit");
            Console.Write("Select an option: ");

            string option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    Console.WriteLine(savingsAccount);
                    break;
                case "2":
                    Console.WriteLine(investmentAccount);
                    break;
                case "3":
                    Console.Write("Enter amount to deposit: ");
                    decimal depositAmount = decimal.Parse(Console.ReadLine());
                    savingsAccount.Deposit(depositAmount);
                    Console.WriteLine($"Deposited {depositAmount:C} to Savings Account.");
                    break;
                case "4":
                    Console.Write("Enter amount to withdraw: ");
                    decimal withdrawAmount = decimal.Parse(Console.ReadLine());
                    savingsAccount.Withdraw(withdrawAmount);
                    Console.WriteLine($"Withdrew {withdrawAmount:C} from Savings Account.");
                    break;
                case "5":
                    savingsAccount.ApplyInterest();
                    Console.WriteLine("Interest applied to Savings Account.");
                    break;
                case "6":
                    Console.Write("Enter stock name: ");
                    string stockName = Console.ReadLine();
                    Console.Write("Enter quantity: ");
                    int quantity = int.Parse(Console.ReadLine());
                    Console.Write("Enter price per share: ");
                    decimal pricePerShare = decimal.Parse(Console.ReadLine());
                    investmentAccount.BuyStock(stockName, quantity, pricePerShare);
                    break;
                case "7":
                    Console.WriteLine("Stock Orders:");
                    foreach (var order in investmentAccount.StockOrders)
                    {
                        Console.WriteLine(order);
                    }
                    break;
                case "8":
                    Console.WriteLine("Exiting...");
                    return; // Exit the program
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
    public class Client
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public List<Account> Accounts { get; private set; }

        public Client(string fullName, string address)
        {
            FullName = fullName;
            Address = address;
            Accounts = new List<Account>();
        }

        public void AddAccount(Account account)
        {
            Accounts.Add(account);
        }
    }

    public abstract class Account
    {
        public decimal Balance { get; protected set; }

        public Account(decimal initialBalance)
        {
            Balance = initialBalance;
        }

        public virtual void Deposit(decimal amount)
        {
            Balance += amount;
        }

        public virtual void Withdraw(decimal amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
            }
            else
            {
                Console.WriteLine("Insufficient funds.");
            }
        }

        public override string ToString()
        {
            return $"Account balance: {Balance:C}";
        }
    }

    public class SavingsAccount : Account
    {
        public decimal InterestRate { get; private set; }

        public SavingsAccount(decimal initialBalance, decimal interestRate)
            : base(initialBalance)
        {
            InterestRate = interestRate;
        }

        public void ApplyInterest()
        {
            Balance += Balance * InterestRate;
        }

        public override string ToString()
        {
            return $"Savings Account: {base.ToString()}, Interest Rate: {InterestRate:P}";
        }
    }

    public class InvestmentAccount : Account
    {
        public decimal CommissionRate { get; private set; }
        public List<StockOrder> StockOrders { get; private set; }

        public InvestmentAccount(decimal initialBalance, decimal commissionRate)
            : base(initialBalance)
        {
            CommissionRate = commissionRate;
            StockOrders = new List<StockOrder>();
        }

        public void BuyStock(string stockName, int quantity, decimal pricePerShare)
        {
            decimal totalCost = quantity * pricePerShare;
            decimal commission = totalCost * CommissionRate;

            if (Balance >= totalCost + commission)
            {
                Balance -= totalCost + commission;
                StockOrders.Add(new StockOrder(stockName, quantity, pricePerShare, commission));
                Console.WriteLine($"Bought {quantity} shares of {stockName} at {pricePerShare:C} each with {commission:C} commission.");
            }
            else
            {
                Console.WriteLine("Insufficient funds to complete stock purchase.");
            }
        }

        public override string ToString()
        {
            return $"Investment Account: {base.ToString()}, Commission Rate: {CommissionRate:P}";
        }
    }

    public class StockOrder
    {
        public string StockName { get; private set; }
        public int Quantity { get; private set; }
        public decimal PricePerShare { get; private set; }
        public decimal Commission { get; private set; }

        public StockOrder(string stockName, int quantity, decimal pricePerShare, decimal commission)
        {
            StockName = stockName;
            Quantity = quantity;
            PricePerShare = pricePerShare;
            Commission = commission;
        }

        public override string ToString()
        {
            return $"Stock: {StockName}, Quantity: {Quantity}, Price per Share: {PricePerShare:C}, Commission: {Commission:C}";
        }
    }
}



