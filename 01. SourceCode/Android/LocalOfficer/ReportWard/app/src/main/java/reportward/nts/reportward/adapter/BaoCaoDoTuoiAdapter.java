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
import reportward.nts.reportward.model.TuoiGioiTinhDanTocModel;

public class BaoCaoDoTuoiAdapter extends RecyclerView.Adapter<BaoCaoDoTuoiAdapter.ViewHolder> {
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

    public BaoCaoDoTuoiAdapter(Context ctx, List<TuoiGioiTinhDanTocModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<TuoiGioiTinhDanTocModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }


    @Override
    public BaoCaoDoTuoiAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_bao_cao_tuoi, parent, false);
        v.setBackgroundResource(mBackground);
        BaoCaoDoTuoiAdapter.ViewHolder vh = new BaoCaoDoTuoiAdapter.ViewHolder(v);
        return vh;
    }

    @Override
    public void onBindViewHolder(BaoCaoDoTuoiAdapter.ViewHolder holder, final int position) {
        final TuoiGioiTinhDanTocModel tuoiGioiTinhDanTocModel = listObject.get(position);
        if (position % 2 == 0) {
            holder.itemRow.setBackgroundColor(Color.parseColor("#D1DCFF"));
        } else {
            holder.itemRow.setBackgroundColor(Color.parseColor("#FFFFFF"));
        }
        holder.txtDoiTuong.setText(tuoiGioiTinhDanTocModel.doiTuong);
        holder.txt03.setText(tuoiGioiTinhDanTocModel.doTuoi0Den3);
        holder.txt46.setText(tuoiGioiTinhDanTocModel.doTuoi4Den6);
        holder.txt79.setText(tuoiGioiTinhDanTocModel.doTuoi7Den9);
        holder.txt10.setText(tuoiGioiTinhDanTocModel.doTuoi10);
        holder.txt1112.setText(tuoiGioiTinhDanTocModel.doTuoi11Den12);
        holder.txt1314.setText(tuoiGioiTinhDanTocModel.doTuoi13Den14);
        holder.txt15.setText(tuoiGioiTinhDanTocModel.doTuoi15Duoi16);
        holder.txt1618.setText(tuoiGioiTinhDanTocModel.doTuoi16Duoi18);
        holder.txt18.setText(tuoiGioiTinhDanTocModel.doTuoiTren18);
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
        public TextView txt03, txt46, txt79, txt10, txt1112, txt1314, txt15, txt1618, txt18;
        LinearLayout itemRow;

        public ViewHolder(View v) {
            super(v);
            txtDoiTuong = v.findViewById(R.id.txtDoiTuong);
            txt03 = v.findViewById(R.id.txt03);
            txt46 = v.findViewById(R.id.txt46);
            txt79 = v.findViewById(R.id.txt79);
            txt10 = v.findViewById(R.id.txt10);
            txt1112 = v.findViewById(R.id.txt1112);
            txt1314 = v.findViewById(R.id.txt1314);
            txt15 = v.findViewById(R.id.txt15);
            txt1618 = v.findViewById(R.id.txt1618);
            txt18 = v.findViewById(R.id.txt18);
            itemRow = v.findViewById(R.id.itemRow);
        }
    }
}
