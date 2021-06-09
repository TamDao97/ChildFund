package nts.childprofile.model;

public class UserUploadImageModel {
    private String Content;
    private String UserId;

    public UserUploadImageModel(String content, String userId) {
        Content = content;
        UserId = userId;
    }

    public String getContent() {
        return Content;
    }

    public void setContent(String content) {
        Content = content;
    }

    public String getUserId() {
        return UserId;
    }

    public void setUserId(String userId) {
        UserId = userId;
    }
}
