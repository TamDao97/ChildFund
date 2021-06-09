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
import reportward.nts.reportward.model.DoiTuongXamHaiViewModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class PrisonerListAdapter extends RecyclerView.Adapter<PrisonerListAdapter.ViewHolder> {

    private final int mBackground;
    private List<DoiTuongXamHaiViewModel> listObject = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onEditClick(View view, int position, DoiTuongXamHaiViewModel obj);
        void onDetailClick(View view, int position, DoiTuongXamHaiViewModel obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public TextView txtTenDoiTuong,txtLoaiDoiTuongXamHai;
        public ImageView ivDetail, ivEdit;

        public ViewHolder(View v) {
            super(v);
            txtTenDoiTuong = (TextView) v.findViewById(R.id.txtTenDoiTuong);
            txtLoaiDoiTuongXamHai =(TextView) v.findViewById(R.id.txtLoaiDoiTuongXamHai);
            ivDetail = (ImageView) v.findViewById(R.id.ivDetail);
            ivEdit = (ImageView) v.findViewById(R.id.ivEdit);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public PrisonerListAdapter(Context ctx, List<DoiTuongXamHaiViewModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<DoiTuongXamHaiViewModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public PrisonerListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_prisoner, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final DoiTuongXamHaiViewModel doiTuongXamHaiViewModel = listObject.get(position);
        if (!Utils.isEmpty(doiTuongXamHaiViewModel.hoTen)) {
            holder.txtTenDoiTuong.setText(doiTuongXamHaiViewModel.hoTen);
        } else {
            holder.txtTenDoiTuong.setText("Không biết tên #" + String.valueOf(position + 1));
        }
        holder.txtLoaiDoiTuongXamHai.setText(doiTuongXamHaiViewModel.loaiDoiTuongXamHai);
        // view detail message conversation
        holder.ivDetail.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onDetailClick(view, position, doiTuongXamHaiViewModel);
                }
            }
        });

        holder.ivEdit.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onEditClick(view, position, doiTuongXamHaiViewModel);
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
