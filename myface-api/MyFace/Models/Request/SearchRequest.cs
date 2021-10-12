namespace MyFace.Models.Request
{
    public class SearchRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public virtual string Filters => "";
    }
    
    public class UserSearchRequest : SearchRequest
    {
        private string _search;
        public string Search
        {
            get => _search?.ToLower();
            set => _search = value;
        }

        public UserSearchRequest(string search) => _search = search;
        
        public override string Filters => Search == null ? "" : $"&search={Search}";
    }

    public class PostSearchRequest : SearchRequest
    {
        public int? PostedBy { get; set; }
        public override string Filters => PostedBy == null ? "" : $"&postedBy={PostedBy}";
    }

    public class FeedSearchRequest : PostSearchRequest
    {
        public int? LikedBy { get; set; }
        public int? DislikedBy { get; set; }
        public override string Filters
        {
            get
            {
                var filters = "";

                if (PostedBy != null)
                {
                    filters += $"&postedBy={PostedBy}";
                }
                
                if (LikedBy != null)
                {
                    filters += $"&likedBy={LikedBy}";
                }
                
                if (DislikedBy != null)
                {
                    filters += $"&dislikedBy={DislikedBy}";
                }
                
                return filters;
            }
        }
    }
}