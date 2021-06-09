package reportward.nts.reportward.adapter;

import android.content.Context;
import android.support.v4.text.HtmlCompat;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.balysv.materialripple.MaterialRippleLayout;

import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.CaTuVanResultModel;
import reportward.nts.reportward.model.ViewHolder;

public class  AdapterListSearchReport extends BaseAdapter {
    LayoutInflater layoutInflater;
    static Context context;
    List<CaTuVanResultModel> listChild;
    Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onItemClick(View view, int position, CaTuVanResultModel obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public AdapterListSearchReport(Context context, List<CaTuVanResultModel> listChild) {
        this.context = context;
        this.listChild = listChild;
        layoutInflater = LayoutInflater.from(context);
    }

    @Override
    public int getCount() {
        return listChild.size();
    }

    @Override
    public Object getItem(int position) {
        return listChild.get(position);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    @Override
    public View getView(final int position, View convertView, ViewGroup parent) {
        ViewHolder holder;
        final CaTuVanResultModel caTuVanResultModel = listChild.get(position);
        if (convertView == null) {
            convertView = layoutInflater.inflate(R.layout.item_list_search_report, null);
            holder = new ViewHolder();
            holder.name = (TextView) convertView.findViewById(R.id.txt_Name);
            holder.address = (TextView) convertView.findViewById(R.id.txt_Address);
            holder.time = (TextView) convertView.findViewById(R.id.txt_Time);
            holder.content = (TextView) convertView.findViewById(R.id.txt_Content);
            holder.ivReport = (ImageView) convertView.findViewById(R.id.ivReport);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }

        holder.name.setText(caTuVanResultModel.tenDoiTuong);
        holder.address.setText((Utils.isEmpty(caTuVanResultModel.tinh) ? "" : caTuVanResultModel.tinh)
                + (Utils.isEmpty(caTuVanResultModel.huyen) ? "" : " - " + caTuVanResultModel.huyen)
                + (Utils.isEmpty(caTuVanResultModel.xa) ? "" : " - " + caTuVanResultModel.xa));
        holder.time.setText(DateUtils.ConvertYMDHHmmServerToDMYHHmm(caTuVanResultModel.ngayTuVan));
        holder.content.setText(HtmlCompat.fromHtml(caTuVanResultModel.noiDungTuVan, 0));

        holder.ivReport.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onItemClick(view, position, caTuVanResultModel);

                }
            }
        });

        return convertView;
    }
}
