package reportward.nts.reportward.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import reportward.nts.reportward.R;
import reportward.nts.reportward.common.Utils;
import reportward.nts.reportward.model.ThongTinTreViewModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class ChildListAdapter extends RecyclerView.Adapter<ChildListAdapter.ViewHolder> {

    private final int mBackground;
    private List<ThongTinTreViewModel> listObject = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onDetailClick(View view, int position, ThongTinTreViewModel obj);

//        void onRemoveClick(View view, int position, ThongTinTreViewModel obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public TextView txtTenTre,txtDoiTuongTre;
        public ImageView ivRemove, ivDetail;

        public ViewHolder(View v) {
            super(v);
            txtTenTre = (TextView) v.findViewById(R.id.txtTenTre);
            txtDoiTuongTre = (TextView) v.findViewById(R.id.txtDoiTuongTre);
//            ivRemove = (ImageView) v.findViewById(R.id.ivRemove);
            ivDetail = (ImageView) v.findViewById(R.id.ivDetail);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public ChildListAdapter(Context ctx, List<ThongTinTreViewModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<ThongTinTreViewModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public ChildListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_child, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final ThongTinTreViewModel thongTinTreViewModel = listObject.get(position);
        if (!Utils.isEmpty(thongTinTreViewModel.hoTen)) {
            holder.txtTenTre.setText(thongTinTreViewModel.hoTen);
        } else {
            holder.txtTenTre.setText("Không biết tên #" + String.valueOf(position + 1));
        }
        holder.txtDoiTuongTre.setText(thongTinTreViewModel.doiTuongTre);

//        // view detail message conversation
//        holder.ivRemove.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View view) {
//                if (mOnItemClickListener != null) {
//                    mOnItemClickListener.onRemoveClick(view, position, thongTinTreViewModel);
//                }
//            }
//        });

        holder.ivDetail.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onDetailClick(view, position, thongTinTreViewModel);
                }
            }
        });
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return listObject.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
