package reportward.nts.reportward.adapter;

import android.content.Context;
import android.graphics.Color;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.model.LoaiCaMoiTruongModel;

public class AdapterLoaiCaMoiTruong extends RecyclerView.Adapter<AdapterLoaiCaMoiTruong.ViewHolder> {

    private final int mBackground;
    private Context ctx;
    private final TypedValue mTypedValue = new TypedValue();
    private List<LoaiCaMoiTruongModel> listObject = new ArrayList<>();

    public AdapterLoaiCaMoiTruong(Context ctx, List<LoaiCaMoiTruongModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<LoaiCaMoiTruongModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public AdapterLoaiCaMoiTruong.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_loaica_moitruong, parent, false);
        v.setBackgroundResource(mBackground);
        AdapterLoaiCaMoiTruong.ViewHolder vh = new AdapterLoaiCaMoiTruong.ViewHolder(v);
        return vh;
    }

    @Override
    public void onBindViewHolder(AdapterLoaiCaMoiTruong.ViewHolder holder, int position) {
        final LoaiCaMoiTruongModel loaiCaMoiTruongModel = listObject.get(position);
        if (position % 2 == 0) {
            holder.itemRow.setBackgroundColor(Color.parseColor("#D1DCFF"));
        } else {
            holder.itemRow.setBackgroundColor(Color.parseColor("#FFFFFF"));
        }
        holder.txtDoiTuongTre.setText(loaiCaMoiTruongModel.doiTuong);
        holder.txtSoLuong.setText(loaiCaMoiTruongModel.soLuong);
        holder.txtTyLe.setText(loaiCaMoiTruongModel.tyLe);
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        public TextView txtDoiTuongTre;
        public TextView txtSoLuong;
        public TextView txtTyLe;
        public LinearLayout itemRow;

        public ViewHolder(View v) {
            super(v);
            txtDoiTuongTre = v.findViewById(R.id.txtDoiTuongTre);
            txtSoLuong = v.findViewById(R.id.txtSoLuong);
            txtTyLe = v.findViewById(R.id.txtTyLe);
            itemRow = v.findViewById(R.id.itemRow);

        }
    }

    @Override
    public int getItemCount() {
        return listObject.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
