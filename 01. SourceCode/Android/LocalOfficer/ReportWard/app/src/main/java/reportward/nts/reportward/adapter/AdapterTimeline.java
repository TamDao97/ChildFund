package reportward.nts.reportward.adapter;

import android.content.Context;
import android.support.v4.text.HtmlCompat;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.common.DateUtils;
import reportward.nts.reportward.model.LienHeChiTietModel;
import reportward.nts.reportward.model.ProcessingContentModel;

public class AdapterTimeline extends RecyclerView.Adapter<AdapterTimeline.ViewHolder> {
    private final int mBackground;
    private List<LienHeChiTietModel> listObject = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;

    private AdapterTimeline.OnItemClickListener onItemClickListener;

    public interface OnItemClickListener {
        void onButtonDetailClick(View view, int position, ProcessingContentModel obj);

        void onButtonMoreClick(View view, int position, ProcessingContentModel obj);
    }

    public void SetOnItemClickListener(final AdapterTimeline.OnItemClickListener mItemClickListener) {
        this.onItemClickListener = mItemClickListener;
    }

    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_timeline, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final LienHeChiTietModel processingContentModel = listObject.get(position);
        holder.tvTime.setText(DateUtils.ConvertYMDHHmmServerToDMYHHmm(processingContentModel.ngayTuVan));
        holder.tvName.setText(processingContentModel.nguoiTuVan);
        holder.tvContent.setText(HtmlCompat.fromHtml(processingContentModel.noiDungTuVan, 0));
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
        public TextView tvTime;
        public TextView tvContent;
        public TextView tvName;
        //public ImageView imageView;

        public ViewHolder(View v) {
            super(v);
            tvTime = (TextView) v.findViewById(R.id.tv_Time);
            tvContent = (TextView) v.findViewById(R.id.tv_Content);
            tvName = (TextView) v.findViewById(R.id.tvName);
        }
    }

    public AdapterTimeline(Context ctx, List<LienHeChiTietModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<LienHeChiTietModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }


}
