package nts.swipesafe.model;

import java.util.List;

/**
 * Created by NTS-VANVV on 29/01/2019.
 */

public class ReportModel extends ReporterModel {
    /// <summary>
    /// Danh sách trẻ bị xâm hại
    /// </summary>
    public List<ChildModel> ListChild;

    /// <summary>
    /// Danh sách nghi phạm
    /// </summary>
    public List<PrisonerModel> ListPrisoner;
}
