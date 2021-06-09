package nts.childprofile.model;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by NTS-VANVV on 27/12/2018.
 */

public class SearchResultObject<T> {
    public int TotalItem;

    public List<T> ListResult = new ArrayList<>();

    public String PathFile;
}
