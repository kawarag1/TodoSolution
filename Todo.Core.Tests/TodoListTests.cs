using Xunit;
using Todo.Core;
using System.Linq;

namespace Todo.Core.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void Add_IncreasesCount()
        {
            var list = new TodoList();
            list.Add(" task ");
            Assert.Equal(1, list.Count);
            Assert.Equal("task", list.Items.First().Title);
        }
        [Fact]
        public void Remove_ById_Works()
        {
            var list = new TodoList();
            var item = list.Add("a");
            Assert.True(list.Remove(item.Id));
            Assert.Equal(0, list.Count);
        }
        [Fact]
        public void Find_ReturnsMatches()
        {
            var list = new TodoList();
            list.Add("Buy milk");
            list.Add("Read book");
            var found = list.Find("buy").ToList();
            Assert.Single(found);
            Assert.Equal("Buy milk", found[0].Title);
        }

        [Fact]
        public void Save_NewJsonFile()
        {
            var todoList = new TodoList();
            var item1 = todoList.Add("Buy groceries");
            var item2 = todoList.Add("Walk the dog");
            item1.MarkDone();

            string testFile = "test_save.json";

            todoList.Save(testFile);

            Assert.True(File.Exists(testFile));

            string jsonContent = File.ReadAllText(testFile);
            Assert.Contains("Buy groceries", jsonContent);
            Assert.Contains("Walk the dog", jsonContent);
            Assert.Contains("isDone", jsonContent);
            Assert.Contains(item1.Id.ToString(), jsonContent);

            File.Delete(testFile);
        }

        [Fact]
        public void Load_FromJsonFile()
        {
            var originalList = new TodoList();
            var originalItem = originalList.Add("Read a book");
            originalItem.MarkDone();
            Guid originalId = originalItem.Id;

            string testFile = "test_load.json";
            originalList.Save(testFile);

            var loadedList = new TodoList();
            loadedList.Load(testFile);

            Assert.Equal(1, loadedList.Count);

            var loadedItem = loadedList.GetItem(originalId);
            Assert.NotNull(loadedItem);
            Assert.Equal("Read a book", loadedItem.Title);
            Assert.True(loadedItem.IsDone);
            Assert.Equal(originalId, loadedItem.Id);

            File.Delete(testFile);
        }
    }
}