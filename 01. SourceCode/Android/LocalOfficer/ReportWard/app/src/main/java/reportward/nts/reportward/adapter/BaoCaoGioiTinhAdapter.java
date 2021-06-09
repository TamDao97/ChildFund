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
import reportward.nts.reportward.model.TuoiGioiTinhDanTocModel;

public class BaoCaoGioiTinhAdapter extends RecyclerView.Adapter<BaoCaoGioiTinhAdapter.ViewHolder> {
    private final int mBackground;
    private Context ctx;
    private final TypedValue mTypedValue = new TypedValue();
    private List<TuoiGioiTinhDanTocModel> listObject = new ArrayList<>();
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void showDialogDetail(View view, int position, TuoiGioiTinhDanTocModel obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public BaoCaoGioiTinhAdapter(Context ctx, List<TuoiGioiTinhDanTocModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<TuoiGioiTinhDanTocModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }


    @Override
    public BaoCaoGioiTinhAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_bao_cao_gioi_tinh, parent, false);
        v.setBackgroundResource(mBackground);
        BaoCaoGioiTinhAdapter.ViewHolder vh = new BaoCaoGioiTinhAdapter.ViewHolder(v);
        return vh;
    }

    @Override
    public void onBindViewHolder(BaoCaoGioiTinhAdapter.ViewHolder holder, final int position) {
        final TuoiGioiTinhDanTocModel tuoiGioiTinhDanTocModel = listObject.get(position);
        if (position % 2 == 0) {
            holder.itemRow.setBackgroundColor(Color.parseColor("#D1DCFF"));
        } else {
            holder.itemRow.setBackgroundColor(Color.parseColor("#FFFFFF"));
        }
        holder.txtDoiTuong.setText(tuoiGioiTinhDanTocModel.doiTuong);
        holder.txtNam.setText(tuoiGioiTinhDanTocModel.gioiTinhNam);
        holder.txtNu.setText(tuoiGioiTinhDanTocModel.gioiTinhNu);
        holder.txtKhac.setText(tuoiGioiTinhDanTocModel.gioiTinhKhongXacDinh);
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
        public TextView txtDoiTuong;
        public TextView txtNam, txtNu, txtKhac;
        LinearLayout itemRow;

        public ViewHolder(View v) {
            super(v);
            txtDoiTuong = v.findViewById(R.id.txtDoiTuong);
            txtNam = v.findViewById(R.id.txtNam);
            txtNu = v.findViewById(R.id.txtNu);
            txtKhac = v.findViewById(R.id.txtKhac);
            itemRow = v.findViewById(R.id.itemRow);
        }
    }
}
