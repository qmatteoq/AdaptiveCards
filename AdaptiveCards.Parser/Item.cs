using System;

namespace AdaptiveCards.Parser
{
    /// <summary>
    /// Represents a feed item.
    /// </summary>
    public class Item
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public FeedType FeedType { get; set; }

        public Item()
        {
            Link = "";
            Title = "";
            Content = "";
            PublishDate = DateTime.Today;
            FeedType = FeedType.RSS;
        }
    }
}
