package reportward.nts.reportward.model;

import java.util.ArrayList;
import java.util.List;

public class SearchResultObject<T> {
    public int TotalItem;

    public List<T> ListResult = new ArrayList<>();

    public String PathFile;
}
