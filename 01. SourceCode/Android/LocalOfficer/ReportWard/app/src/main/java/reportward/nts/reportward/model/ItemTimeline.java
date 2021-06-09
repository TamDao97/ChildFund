package reportward.nts.reportward.model;

public class ItemTimeline {
    private String Id;
    private String tvTime;
    private String Content;

    public ItemTimeline(String id, String tvTime, String content) {
        Id = id;
        this.tvTime = tvTime;
        Content = content;
    }

    public String getId() {
        return Id;
    }

    public void setId(String id) {
        Id = id;
    }

    public String getTvTime() {
        return tvTime;
    }

    public void setTvTime(String tvTime) {
        this.tvTime = tvTime;
    }

    public String getContent() {
        return Content;
    }

    public void setContent(String content) {
        Content = content;
    }
}
