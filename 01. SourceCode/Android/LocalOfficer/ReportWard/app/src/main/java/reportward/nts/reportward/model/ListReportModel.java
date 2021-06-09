package reportward.nts.reportward.model;

public class ListReportModel {
    private String Namel;
    private String Address;
    private String Type;
    private String Time;

    public ListReportModel(String namel, String address, String type, String time) {
        Namel = namel;
        Address = address;
        Type = type;
        Time = time;
    }

    public String getNamel() {
        return Namel;
    }

    public void setNamel(String namel) {
        Namel = namel;
    }

    public String getAddress() {
        return Address;
    }

    public void setAddress(String address) {
        Address = address;
    }

    public String getType() {
        return Type;
    }

    public void setType(String type) {
        Type = type;
    }

    public String getTime() {
        return Time;
    }

    public void setTime(String time) {
        Time = time;
    }
}
