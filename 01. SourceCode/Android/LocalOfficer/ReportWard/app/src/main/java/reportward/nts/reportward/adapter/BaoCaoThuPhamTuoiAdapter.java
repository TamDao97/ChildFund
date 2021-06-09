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
import reportward.nts.reportward.model.BaoCaoThuPhamModel;

public class BaoCaoThuPhamTuoiAdapter extends RecyclerView.Adapter<BaoCaoThuPhamTuoiAdapter.ViewHolder> {
    private final int mBackground;
    private Context ctx;
    private final TypedValue mTypedValue = new TypedValue();
    private List<BaoCaoThuPhamModel> listObject = new ArrayList<>();
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void showDialogDetail(View view, int position, BaoCaoThuPhamModel obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public BaoCaoThuPhamTuoiAdapter(Context ctx, List<BaoCaoThuPhamModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<BaoCaoThuPhamModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }


    @Override
    public BaoCaoThuPhamTuoiAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_bc_thu_pham_do_tuoi, parent, false);
        v.setBackgroundResource(mBackground);
        BaoCaoThuPhamTuoiAdapter.ViewHolder vh = new BaoCaoThuPhamTuoiAdapter.ViewHolder(v);
        return vh;
    }

    @Override
    public void onBindViewHolder(BaoCaoThuPhamTuoiAdapter.ViewHolder holder, final int position) {
        final BaoCaoThuPhamModel baoCaoThuPhamModel = listObject.get(position);
        if (position % 2 == 0) {
            holder.itemRow.setBackgroundColor(Color.parseColor("#D1DCFF"));
        } else {
            holder.itemRow.setBackgroundColor(Color.parseColor("#FFFFFF"));
        }
        holder.txtQuanHeVoiTre.setText(baoCaoThuPhamModel.quanHeVoiTre);
        holder.txtDuoi16.setText(baoCaoThuPhamModel.doTuoiDuoi16);
        holder.txtTu1618.setText(baoCaoThuPhamModel.doTuoi16Duoi18);
        holder.txtTren18.setText(baoCaoThuPhamModel.doTuoiTren18);
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
        public TextView txtQuanHeVoiTre;
        public TextView txtDuoi16, txtTu1618, txtTren18;
        LinearLayout itemRow;

        public ViewHolder(View v) {
            super(v);
            txtQuanHeVoiTre = v.findViewById(R.id.txtQuanHeVoiTre);
            txtDuoi16 = v.findViewById(R.id.txtDuoi16);
            txtTu1618 = v.findViewById(R.id.txtTu1618);
            txtTren18 = v.findViewById(R.id.txtTren18);
            itemRow = v.findViewById(R.id.itemRow);
        }
    }
}
