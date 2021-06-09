package reportward.nts.reportward.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageButton;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.model.CaTuVanResultModel;

public class AdapterListReport extends RecyclerView.Adapter<AdapterListReport.ViewHolder> {
    private final int mBackground;
    private List<CaTuVanResultModel> listObject = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener onItemClickListener;

    public interface OnItemClickListener {
        void onButtonDetailClick(View view, int position, CaTuVanResultModel obj);

        void onButtonReportClick(View view, int position, CaTuVanResultModel obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.onItemClickListener = mItemClickListener;
    }

    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_list_report, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        AdapterListReport.ViewHolder vh = new AdapterListReport.ViewHolder(v);
        return vh;
    }

    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final CaTuVanResultModel caTuVanResultModel = listObject.get(position);
        holder.Name.setText(caTuVanResultModel.tenDoiTuong);
        holder.Address.setText(caTuVanResultModel.diaDiem);
        holder.Type.setText(caTuVanResultModel.noiDungTuVan);
        //DateUtils dateUtils = new DateUtils();
        //String receptionDate = dateUtils.ConvertYMDServerToDMYHHMM(profileChildSearchResult.ReceptionDate);
        holder.Time.setText(caTuVanResultModel.ngayTuVan);

        holder.btnShowDialogReport.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (onItemClickListener != null) {
                    onItemClickListener.onButtonReportClick(view, position, caTuVanResultModel);
                }
            }
        });

        holder.btnShowDetailReport.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (onItemClickListener != null) {
                    onItemClickListener.onButtonDetailClick(view, position, caTuVanResultModel);
                }
            }
        });
    }

    @Override
    public int getItemCount() {
        return listObject.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }


    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public TextView Name;
        public TextView Address;
        public TextView Type;
        public TextView Time;
        public ImageButton btnShowDialogReport;
        public ImageButton btnShowDetailReport;

        public ViewHolder(View v) {
            super(v);
            Name = (TextView) v.findViewById(R.id.txt_Name);
            Address = (TextView) v.findViewById(R.id.txt_Address);
            Type = v.findViewById(R.id.txt_Type);
            Time = v.findViewById(R.id.txt_Time);
            btnShowDialogReport = v.findViewById(R.id.btnShowDialog);
            btnShowDetailReport = v.findViewById(R.id.btnShowDetail);


        }
    }

    public AdapterListReport(Context ctx, List<CaTuVanResultModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<CaTuVanResultModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }
}
