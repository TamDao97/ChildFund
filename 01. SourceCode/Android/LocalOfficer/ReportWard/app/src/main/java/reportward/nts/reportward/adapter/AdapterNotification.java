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
import reportward.nts.reportward.model.InfoNotification;


public class AdapterNotification extends RecyclerView.Adapter<AdapterNotification.ViewHolder> {
    private final int mBackground;
    private List<InfoNotification> listObject = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;

    private OnItemClickListener onItemClickListener;

    public interface OnItemClickListener {
        void onButtonSeenClick(View view, int position, InfoNotification obj);

        void onButtonDeleteClick(View view, int position, InfoNotification obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.onItemClickListener = mItemClickListener;
    }

    @Override
    public AdapterNotification.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_notification, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        AdapterNotification.ViewHolder vh = new AdapterNotification.ViewHolder(v);
        return vh;
    }

    @Override
    public void onBindViewHolder(final AdapterNotification.ViewHolder holder, final int position) {
        final InfoNotification notifyModel = listObject.get(position);
        holder.Title.setText(notifyModel.date);
        holder.Content.setText(notifyModel.massenger);
        if (notifyModel.viewStatus) {
            holder.itemNotify.setBackgroundColor(Color.parseColor("#FFFFFF"));
        }else
        {
            holder.itemNotify.setBackgroundColor(Color.parseColor("#1A000000"));
        }

        holder.itemNotify.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (onItemClickListener != null) {
                    onItemClickListener.onButtonSeenClick(view, position, notifyModel);

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
        public TextView Title;
        //public TextView tvTime;
        public TextView Content;
        //public ImageView btnSeen;
        //public ImageView btnDelete;
        public LinearLayout itemNotify;

        public ViewHolder(View v) {
            super(v);
            Content = (TextView) v.findViewById(R.id.tv_Content);
            Title = v.findViewById(R.id.tv_Header);
            itemNotify = v.findViewById(R.id.itemNotify);
        }
    }

    public AdapterNotification(Context ctx, List<InfoNotification> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<InfoNotification>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }
}
