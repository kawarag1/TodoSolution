using System.Text.Json;
using System.Text.Json.Serialization;

namespace Todo.Core
{
    public class TodoItem
    {
        [JsonPropertyName("id")]
        public Guid Id { get; } = Guid.NewGuid();

        [JsonPropertyName("title")]
        public string Title { get; private set; }
        [JsonPropertyName("isDone")]
        public bool IsDone { get; private set; }

        [JsonConstructor]
        public TodoItem(Guid id, string title, bool isDone)
        {
            Id = id;
            Title = title;
            IsDone = isDone;
        }

        public TodoItem(string title)
        {
            Title = title?.Trim() ?? throw new ArgumentNullException(nameof(title));
        }

        public void MarkDone() => IsDone = true;
        public void MarkUndone() => IsDone = false;
        public void Rename(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle)) throw new ArgumentException("Titlerequired", nameof(newTitle));
           
            Title = newTitle.Trim();
        }
    }
}
