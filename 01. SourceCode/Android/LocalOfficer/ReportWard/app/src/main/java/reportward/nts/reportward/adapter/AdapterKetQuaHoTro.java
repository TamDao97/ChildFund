package reportward.nts.reportward.adapter;

import android.content.Context;
import android.graphics.Color;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.LinearLayout;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.model.KetQuaHoTroModel;

public class AdapterKetQuaHoTro extends RecyclerView.Adapter<AdapterKetQuaHoTro.ViewHolder> {
    private final int mBackground;
    private Context ctx;
    private final TypedValue mTypedValue = new TypedValue();
    private List<KetQuaHoTroModel> listObject = new ArrayList<>();
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void showDialogDetail(View view, int position, KetQuaHoTroModel obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public AdapterKetQuaHoTro(Context ctx, List<KetQuaHoTroModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<KetQuaHoTroModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }


    @Override
    public AdapterKetQuaHoTro.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_ketqua_hotro, parent, false);
        v.setBackgroundResource(mBackground);
        AdapterKetQuaHoTro.ViewHolder vh = new AdapterKetQuaHoTro.ViewHolder(v);
        return vh;
    }

    @Override
    public void onBindViewHolder(AdapterKetQuaHoTro.ViewHolder holder, final int position) {
        final KetQuaHoTroModel ketQuaHoTroModel = listObject.get(position);
        holder.txtDoiTuongTre.setText(ketQuaHoTroModel.doiTuong);

        if (position % 2 == 0) {
            holder.itemRow.setBackgroundColor(Color.parseColor("#D1DCFF"));
        } else {
            holder.itemRow.setBackgroundColor(Color.parseColor("#FFFFFF"));
        }

        int coHoTro = 0;
        coHoTro = Integer.parseInt(ketQuaHoTroModel.coHoTroChamSocSoLuong)
                + Integer.parseInt(ketQuaHoTroModel.coHoTroTroGiupGiaoDucSoLuong)
                + Integer.parseInt(ketQuaHoTroModel.coHoTroTroGiupKhacSoLuong)
                + Integer.parseInt(ketQuaHoTroModel.coHoTroTroGiupPhapLySoLuong)
                + Integer.parseInt(ketQuaHoTroModel.coHoTroTroGiupTaiChinhSoLuong)
                + Integer.parseInt(ketQuaHoTroModel.coHoTroTroGiupTamLySoLuong)
                + Integer.parseInt(ketQuaHoTroModel.coHoTroTroGiupXaHoiSoLuong);
        holder.txtHoTro.setText(String.valueOf(coHoTro));

        int khongHoTro = 0;
        khongHoTro = Integer.parseInt(ketQuaHoTroModel.koHoTroGiaDinhKhongMuonSoLuong)
                + Integer.parseInt(ketQuaHoTroModel.koHoTroKhacSoLuong)
                + Integer.parseInt(ketQuaHoTroModel.koHoTroNanNhanChuyenDiSoLuong);
        holder.txtKhongHoTro.setText(String.valueOf(khongHoTro));

        holder.txtXacMinhKhac.setText(ketQuaHoTroModel.xacMinhKhongCoSoLuong);

        holder.itemRow.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.showDialogDetail(view, position, ketQuaHoTroModel);
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
        public TextView txtDoiTuongTre;
        public TextView txtHoTro;
        public TextView txtKhongHoTro;
        public TextView txtXacMinhKhac;
        LinearLayout itemRow;

        public ViewHolder(View v) {
            super(v);
            txtDoiTuongTre = v.findViewById(R.id.txtDoiTuongTre);
            txtHoTro = v.findViewById(R.id.txtHoTro);
            txtKhongHoTro = v.findViewById(R.id.txtKhongHoTro);
            txtXacMinhKhac = v.findViewById(R.id.txtXacMinhKhac);
            itemRow = v.findViewById(R.id.itemRow);
        }
    }
}
