using System.Security.Cryptography.X509Certificates;

namespace RPGGame
{
    public class Inventory {

        // define Inventory as an items dictionary, key: item name, value: item count
        private Dictionary<string, int> items = new Dictionary<string, int>();

        // Inventory methods
        public void Status() {
            if (items.Count == 0) {
                Console.WriteLine("Inventory empty");
            }
            else {
                Console.WriteLine("Inventory is not empty");
            }
        }

        public void AddItem(Item item) {
            if (items.ContainsKey(item.GetName())) {
                items[item.GetName()] += 1;
            }
            else {
                items.Add(item.GetName(), 1);
            }
        }

        public void Show() {

            foreach (var item in items) {
            Console.WriteLine($"{item.Key} x{item.Value}");
            }

        }
    }
}