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
import reportward.nts.reportward.model.BaoCaoDiaBanModel;

public class BaoCaoDiaBanAdapter extends RecyclerView.Adapter<BaoCaoDiaBanAdapter.ViewHolder> {
    private final int mBackground;
    private Context ctx;
    private final TypedValue mTypedValue = new TypedValue();
    private List<BaoCaoDiaBanModel> listObject = new ArrayList<>();
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void showDialogDetail(View view, int position, BaoCaoDiaBanModel obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public BaoCaoDiaBanAdapter(Context ctx, List<BaoCaoDiaBanModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<BaoCaoDiaBanModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }


    @Override
    public BaoCaoDiaBanAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_bao_cao_dia_ban, parent, false);
        v.setBackgroundResource(mBackground);
        BaoCaoDiaBanAdapter.ViewHolder vh = new BaoCaoDiaBanAdapter.ViewHolder(v);
        return vh;
    }

    @Override
    public void onBindViewHolder(BaoCaoDiaBanAdapter.ViewHolder holder, final int position) {
        final BaoCaoDiaBanModel baoCaoDiaBanModel = listObject.get(position);
        if (position % 2 == 0) {
            holder.itemRow.setBackgroundColor(Color.parseColor("#D1DCFF"));
        } else {
            holder.itemRow.setBackgroundColor(Color.parseColor("#FFFFFF"));
        }
        holder.txtDoiTuong.setText(baoCaoDiaBanModel.doiTuong);
        holder.txtDiaBan.setText(baoCaoDiaBanModel.tinh);
        holder.txtSoLuong.setText(baoCaoDiaBanModel.soLuong);
        holder.txtTyLe.setText(baoCaoDiaBanModel.tyLe);
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
        public TextView txtDiaBan;
        public TextView txtSoLuong;
        public TextView txtTyLe;
        LinearLayout itemRow;

        public ViewHolder(View v) {
            super(v);
            txtDoiTuong = v.findViewById(R.id.txtDoiTuong);
            txtDiaBan = v.findViewById(R.id.txtDiaBan);
            txtSoLuong = v.findViewById(R.id.txtSoLuong);
            txtTyLe = v.findViewById(R.id.txtTyLe);
            itemRow = v.findViewById(R.id.itemRow);
        }
    }
}
